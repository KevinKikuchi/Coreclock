using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Coreclock
{
    // ── USER PROFILE MODEL ───────────────────────────────────────────────────
    public class UserProfile
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = "";

        [JsonPropertyName("full_name")]
        public string FullName { get; set; } = "";

        [JsonPropertyName("email")]
        public string Email { get; set; } = "";

        [JsonPropertyName("contact_number")]
        public string ContactNumber { get; set; } = "";

        [JsonPropertyName("employee_id")]
        public string EmployeeId { get; set; } = "";

        [JsonPropertyName("position")]
        public string Position { get; set; } = "Agent";

        [JsonPropertyName("role")]
        public string Role { get; set; } = "employee";

        [JsonPropertyName("created_at")]
        public string? CreatedAt { get; set; }

        [JsonPropertyName("work_days")]
        public string WorkDays { get; set; } = "Mon-Fri";

        [JsonPropertyName("shift_type")]
        public string ShiftType { get; set; } = "Morning";

        [JsonPropertyName("shift_time_in")]
        public string ShiftTimeIn { get; set; } = "08:00 AM";

        [JsonPropertyName("shift_time_out")]
        public string ShiftTimeOut { get; set; } = "05:00 PM";
    }

    public class AttendanceLog
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = "";

        [JsonPropertyName("user_id")]
        public string UserId { get; set; } = "";

        [JsonPropertyName("date")]
        public string Date { get; set; } = "";

        [JsonPropertyName("time_in")]
        public string? TimeIn { get; set; }

        [JsonPropertyName("time_out")]
        public string? TimeOut { get; set; }

        [JsonPropertyName("total_hours")]
        public string? TotalHours { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = "";
    }

    // ── AUTH RESPONSE MODELS ─────────────────────────────────────────────────
    public class AuthResponse
    {
        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }

        [JsonPropertyName("refresh_token")]
        public string? RefreshToken { get; set; }

        [JsonPropertyName("user")]
        public AuthUser? User { get; set; }
    }

    public class AuthUser
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = "";

        [JsonPropertyName("email")]
        public string Email { get; set; } = "";

        [JsonPropertyName("email_confirmed_at")]
        public string? EmailConfirmedAt { get; set; }
    }

    // ── SUPABASE HELPER (Singleton) ──────────────────────────────────────────
    public class SupabaseHelper
    {
        private const string SUPABASE_URL = "https://xewxdodeextonxnbpnac.supabase.co";
        private const string SUPABASE_ANON_KEY = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Inhld3hkb2RlZXh0b254bmJwbmFjIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NzgwMzcwNjIsImV4cCI6MjA5MzYxMzA2Mn0.vek_ziV5lcRWoRh2Hgyw3FHZRiQ33379GmunyiTl3pY";

        // ── Singleton ──
        private static SupabaseHelper? _instance;
        public static SupabaseHelper Instance => _instance ??= new SupabaseHelper();

        // ── State ──
        private readonly HttpClient _http;
        public AuthResponse? CurrentSession { get; private set; }
        public UserProfile? CurrentUserProfile { get; private set; }

        private SupabaseHelper()
        {
            _http = new HttpClient();
            _http.DefaultRequestHeaders.Add("apikey", SUPABASE_ANON_KEY);
        }

        // ── REGISTER ────────────────────────────────────────────────────────
        /// <summary>
        /// Creates a new employee account.
        /// Supabase sends a verification email automatically.
        /// Returns (success, error).
        /// </summary>
        public async Task<(bool success, string? error)> SignUpAsync(
            string email,
            string password,
            string fullName,
            string contactNumber)
        {
            try
            {
                var payload = new
                {
                    email,
                    password,
                    data = new
                    {
                        full_name      = fullName,
                        contact_number = contactNumber,
                        position       = "Agent"   // default position
                    }
                };

                var json    = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _http.PostAsync(
                    $"{SUPABASE_URL}/auth/v1/signup", content);

                var responseBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var err = JsonSerializer.Deserialize<JsonElement>(responseBody);
                    string msg = err.TryGetProperty("msg", out var m) ? m.GetString()! :
                                 err.TryGetProperty("message", out var m2) ? m2.GetString()! :
                                 "Unknown error";
                    return (false, msg);
                }

                // Success — user must verify email before they can log in
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        // ── LOGIN ────────────────────────────────────────────────────────────
        /// <summary>
        /// Signs in with email + password.
        /// Returns (success, role, error).
        /// </summary>
        public async Task<(bool success, string? role, string? error)> SignInAsync(
            string email,
            string password)
        {
            try
            {
                var payload = new { email, password };
                var json    = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _http.PostAsync(
                    $"{SUPABASE_URL}/auth/v1/token?grant_type=password", content);

                var responseBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var err = JsonSerializer.Deserialize<JsonElement>(responseBody);
                    string msg = err.TryGetProperty("error_description", out var m) ? m.GetString()! :
                                 err.TryGetProperty("message", out var m2) ? m2.GetString()! :
                                 "Invalid credentials";
                    return (false, null, msg);
                }

                var auth = JsonSerializer.Deserialize<AuthResponse>(responseBody);
                if (auth?.AccessToken == null)
                    return (false, null, "Login failed — no session returned.");

                CurrentSession = auth;

                var profile = await FetchProfileAsync(auth.User.Id, auth.AccessToken);
                if (profile == null)
                    return (false, null, "Could not load your profile. Please contact support.");

                CurrentUserProfile = profile;
                return (true, profile.Role, null);
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        // ── FETCH PROFILE ────────────────────────────────────────────────────
        private async Task<UserProfile?> FetchProfileAsync(string userId, string accessToken)
        {
            try
            {
                var request = new HttpRequestMessage(
                    HttpMethod.Get,
                    $"{SUPABASE_URL}/rest/v1/users?id=eq.{userId}&select=*");

                request.Headers.Add("Authorization", $"Bearer {accessToken}");
                request.Headers.Add("apikey", SUPABASE_ANON_KEY);

                var response = await _http.SendAsync(request);
                var body     = await response.Content.ReadAsStringAsync();

                System.Diagnostics.Debug.WriteLine($"🔍 FetchProfileAsync Response: {body}");

                if (!response.IsSuccessStatusCode) return null;

                var profiles = JsonSerializer.Deserialize<List<UserProfile>>(body);
                return profiles?.Count > 0 ? profiles[0] : null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ FetchProfileAsync Error: {ex.Message}");
                return null;
            }
        }

        // ── REFRESH MY PROFILE (public) ─────────────────────────────────────
        /// <summary>
        /// Re-fetches the current user's profile from DB to get latest data (e.g. schedule changes).
        /// </summary>
        public async Task<UserProfile?> RefreshMyProfileAsync()
        {
            if (CurrentSession?.AccessToken == null || CurrentSession?.User?.Id == null)
                return null;

            var profile = await FetchProfileAsync(CurrentSession.User.Id, CurrentSession.AccessToken);
            if (profile != null)
                CurrentUserProfile = profile;
            return profile;
        }

        // ── LOGOUT ───────────────────────────────────────────────────────────
        public async Task SignOutAsync()
        {
            try
            {
                if (CurrentSession?.AccessToken == null) return;

                var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    $"{SUPABASE_URL}/auth/v1/logout");
                request.Headers.Add("Authorization", $"Bearer {CurrentSession.AccessToken}");

                await _http.SendAsync(request);
            }
            catch { }
            finally
            {
                CurrentSession     = null;
                CurrentUserProfile = null;
            }
        }

        // ── ERROR HELPER ─────────────────────────────────────────────────────
        public bool IsInvalidCredentialsError(string? error)
        {
            if (error == null) return false;
            return error.Contains("Invalid login", StringComparison.OrdinalIgnoreCase) ||
                   error.Contains("invalid credentials", StringComparison.OrdinalIgnoreCase) ||
                   error.Contains("Email not confirmed", StringComparison.OrdinalIgnoreCase);
        }
        // ── FETCH ALL EMPLOYEES ──────────────────────────────────────────────
        /// <summary>
        /// Fetches all users from the public.users table.
        /// Used by AdminDashboard to populate the employee list.
        /// </summary>
        public async Task<List<UserProfile>> FetchAllEmployeesAsync()
        {
            try
            {
                var request = new HttpRequestMessage(
                    HttpMethod.Get,
                    $"{SUPABASE_URL}/rest/v1/users?select=*&order=full_name.asc&role=eq.employee");

                request.Headers.Add("Authorization", $"Bearer {CurrentSession?.AccessToken}");
                request.Headers.Add("apikey", SUPABASE_ANON_KEY);

                var response = await _http.SendAsync(request);
                var body = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode) return new List<UserProfile>();

                var users = JsonSerializer.Deserialize<List<UserProfile>>(body);
                return users ?? new List<UserProfile>();
            }
            catch { return new List<UserProfile>(); }
        }
        // ── SAVE SCHEDULE ───────────────────────────────────────────────────────
        /// <summary>
        /// Saves schedule data for a specific employee by id (UUID).
        /// </summary>
        public async Task<(bool success, string error)> SaveScheduleAsync(string userId, string workDays, string shiftType, string timeIn, string timeOut)
        {
            try
            {
                var payload = new
                {
                    work_days = workDays,
                    shift_type = shiftType,
                    shift_time_in = timeIn,
                    shift_time_out = timeOut
                };

                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Use UUID (id) instead of employee_id
                var request = new HttpRequestMessage(
                    HttpMethod.Patch,
                    $"{SUPABASE_URL}/rest/v1/users?id=eq.{userId}");

                request.Headers.Add("Authorization", $"Bearer {CurrentSession?.AccessToken}");
                request.Headers.Add("apikey", SUPABASE_ANON_KEY);
                request.Headers.Add("Prefer", "return=minimal");
                request.Content = content;

                var response = await _http.SendAsync(request);
                var body = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return (false, $"HTTP {(int)response.StatusCode}: {body}");

                return (true, "");
            }
            catch (Exception ex) { return (false, ex.Message); }
        }

        // ── TIME IN ─────────────────────────────────────────────────────────────────
        public async Task<(bool success, string timeIn, string error)> TimeInAsync(string userId)
        {
            try
            {
                string today = DateTime.Now.ToString("yyyy-MM-dd");
                string timeNow = DateTime.Now.ToString("hh:mm tt");

                var checkRequest = new HttpRequestMessage(
                    HttpMethod.Get,
                    $"{SUPABASE_URL}/rest/v1/attendance_logs?user_id=eq.{userId}&date=eq.{today}&select=*");
                checkRequest.Headers.Add("Authorization", $"Bearer {CurrentSession?.AccessToken}");
                checkRequest.Headers.Add("apikey", SUPABASE_ANON_KEY);

                var checkResponse = await _http.SendAsync(checkRequest);
                var checkBody = await checkResponse.Content.ReadAsStringAsync();

                List<JsonElement>? existing = null;
                try { existing = JsonSerializer.Deserialize<List<JsonElement>>(checkBody); }
                catch { return (false, "", "Already timed in today."); }

                if (existing?.Count > 0)
                    return (false, "", "Already timed in today.");

                var payload = new
                {
                    user_id = userId,
                    date = today,
                    time_in = timeNow,
                    status = "Present"
                };

                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Post,
                    $"{SUPABASE_URL}/rest/v1/attendance_logs");
                request.Headers.Add("Authorization", $"Bearer {CurrentSession?.AccessToken}");
                request.Headers.Add("apikey", SUPABASE_ANON_KEY);
                request.Headers.Add("Prefer", "return=minimal");
                request.Content = content;

                var response = await _http.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    var err = await response.Content.ReadAsStringAsync();
                    return (false, "", err);
                }

                return (true, timeNow, "");
            }
            catch (Exception ex) { return (false, "", ex.Message); }
        }

        // ── TIME OUT ─────────────────────────────────────────────────────────────────
        public async Task<(bool success, string timeOut, string totalHours, string error)> TimeOutAsync(string userId)
        {
            try
            {
                string today = DateTime.Now.ToString("yyyy-MM-dd");
                string timeNow = DateTime.Now.ToString("hh:mm tt");

                var checkRequest = new HttpRequestMessage(
                    HttpMethod.Get,
                    $"{SUPABASE_URL}/rest/v1/attendance_logs?user_id=eq.{userId}&date=eq.{today}&select=*");
                checkRequest.Headers.Add("Authorization", $"Bearer {CurrentSession?.AccessToken}");
                checkRequest.Headers.Add("apikey", SUPABASE_ANON_KEY);

                var checkResponse = await _http.SendAsync(checkRequest);
                var checkBody = await checkResponse.Content.ReadAsStringAsync();

                List<JsonElement>? logs = null;
                try { logs = JsonSerializer.Deserialize<List<JsonElement>>(checkBody); }
                catch { return (false, "", "", "No time-in record found for today."); }

                if (logs == null || logs.Count == 0)
                    return (false, "", "", "No time-in record found for today.");

                var log = logs[0];
                if (log.TryGetProperty("time_out", out var to) &&
                    to.ValueKind != JsonValueKind.Null &&
                    !string.IsNullOrEmpty(to.GetString()))
                    return (false, "", "", "Already timed out today.");

                string timeIn = log.GetProperty("time_in").GetString() ?? "";
                double totalHours = 0;
                if (DateTime.TryParse(timeIn, out var inTime) &&
                    DateTime.TryParse(timeNow, out var outTime))
                    totalHours = (outTime - inTime).TotalHours;

                string totalStr = $"{(int)totalHours}h {(int)((totalHours % 1) * 60)}m";
                string logId = log.GetProperty("id").GetString() ?? "";

                var patchPayload = new { time_out = timeNow, total_hours = totalStr, status = "Offline" };
                var json = JsonSerializer.Serialize(patchPayload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Patch,
                    $"{SUPABASE_URL}/rest/v1/attendance_logs?id=eq.{logId}");
                request.Headers.Add("Authorization", $"Bearer {CurrentSession?.AccessToken}");
                request.Headers.Add("apikey", SUPABASE_ANON_KEY);
                request.Headers.Add("Prefer", "return=minimal");
                request.Content = content;

                var response = await _http.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    var err = await response.Content.ReadAsStringAsync();
                    return (false, "", "", err);
                }

                return (true, timeNow, totalStr, "");
            }
            catch (Exception ex) { return (false, "", "", ex.Message); }
        }

        // ── FETCH MY ATTENDANCE LOGS ─────────────────────────────────────────────────
        public async Task<List<AttendanceLog>> FetchMyLogsAsync(string userId)
        {
            try
            {
                var request = new HttpRequestMessage(
                    HttpMethod.Get,
                    $"{SUPABASE_URL}/rest/v1/attendance_logs?user_id=eq.{userId}&order=date.desc&limit=30&select=*");
                request.Headers.Add("Authorization", $"Bearer {CurrentSession?.AccessToken}");
                request.Headers.Add("apikey", SUPABASE_ANON_KEY);

                var response = await _http.SendAsync(request);
                var body = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode) return new List<AttendanceLog>();

                return JsonSerializer.Deserialize<List<AttendanceLog>>(body) ?? new List<AttendanceLog>();
            }
            catch { return new List<AttendanceLog>(); }
        }

        // ── FETCH ALL ATTENDANCE LOGS BY DATE ───────────────────────────────────
        public async Task<List<AttendanceLog>> FetchAllAttendanceLogsAsync(string date)
        {
            try
            {
                var request = new HttpRequestMessage(
                    HttpMethod.Get,
                    $"{SUPABASE_URL}/rest/v1/attendance_logs?date=eq.{date}&select=*");
                request.Headers.Add("Authorization", $"Bearer {CurrentSession?.AccessToken}");
                request.Headers.Add("apikey", SUPABASE_ANON_KEY);

                var response = await _http.SendAsync(request);
                var body = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode) return new List<AttendanceLog>();
                return JsonSerializer.Deserialize<List<AttendanceLog>>(body) ?? new List<AttendanceLog>();
            }
            catch { return new List<AttendanceLog>(); }
        }

        // ── DELETE TODAY'S ABSENT RECORD ─────────────────────────────────────────
        public async Task DeleteTodayAbsentAsync(string userId)
        {
            try
            {
                string today = DateTime.Now.ToString("yyyy-MM-dd");
                var request = new HttpRequestMessage(
                    HttpMethod.Delete,
                    $"{SUPABASE_URL}/rest/v1/attendance_logs?user_id=eq.{userId}&date=eq.{today}&status=eq.Absent");
                request.Headers.Add("Authorization", $"Bearer {CurrentSession?.AccessToken}");
                request.Headers.Add("apikey", SUPABASE_ANON_KEY);
                await _http.SendAsync(request);
            }
            catch { }
        }

        // ── AUTO-ABSENT ───────────────────────────────────────────────────────────
        public async Task MarkAbsentIfLateAsync()
        {
            try
            {
                string today = DateTime.Now.ToString("yyyy-MM-dd");
                var employees = await FetchAllEmployeesAsync();

                foreach (var emp in employees)
                {
                    var checkRequest = new HttpRequestMessage(
                        HttpMethod.Get,
                        $"{SUPABASE_URL}/rest/v1/attendance_logs?user_id=eq.{emp.Id}&date=eq.{today}&select=id");
                    checkRequest.Headers.Add("Authorization", $"Bearer {CurrentSession?.AccessToken}");
                    checkRequest.Headers.Add("apikey", SUPABASE_ANON_KEY);

                    var checkResponse = await _http.SendAsync(checkRequest);
                    var checkBody = await checkResponse.Content.ReadAsStringAsync();
                    var existing = JsonSerializer.Deserialize<List<JsonElement>>(checkBody);
                    if (existing?.Count > 0) continue;

                    if (!DateTime.TryParse(emp.ShiftTimeIn, out var shiftStart)) continue;
                    var cutoff = DateTime.Today.Add(shiftStart.TimeOfDay).AddHours(1);
                    if (DateTime.Now < cutoff) continue;

                    var payload = new
                    {
                        user_id = emp.Id,
                        date = today,
                        status = "Absent"
                    };

                    var json = JsonSerializer.Serialize(payload);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var request = new HttpRequestMessage(HttpMethod.Post,
                        $"{SUPABASE_URL}/rest/v1/attendance_logs");
                    request.Headers.Add("Authorization", $"Bearer {CurrentSession?.AccessToken}");
                    request.Headers.Add("apikey", SUPABASE_ANON_KEY);
                    request.Headers.Add("Prefer", "return=minimal");
                    request.Content = content;

                    await _http.SendAsync(request);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ MarkAbsentIfLateAsync: {ex.Message}");
            }
        }

        // ── REPORT MODEL ─────────────────────────────────────────────────────────────
        public class ReportModel
        {
            [JsonPropertyName("id")]
            public string Id { get; set; } = "";

            [JsonPropertyName("user_id")]
            public string UserId { get; set; } = "";

            [JsonPropertyName("employee_id")]
            public string EmployeeId { get; set; } = "";

            [JsonPropertyName("full_name")]
            public string FullName { get; set; } = "";

            [JsonPropertyName("report_type")]
            public string ReportType { get; set; } = "";

            [JsonPropertyName("message")]
            public string Message { get; set; } = "";

            [JsonPropertyName("is_read")]
            public bool IsRead { get; set; } = false;

            [JsonPropertyName("created_at")]
            public string? CreatedAt { get; set; }
        }

        // ── SUBMIT REPORT ─────────────────────────────────────────────────────────────
        public async Task<(bool success, string error)> SubmitReportAsync(
            string reportType, string message)
        {
            try
            {
                var profile = CurrentUserProfile;
                if (profile == null) return (false, "No profile found.");

                var payload = new
                {
                    user_id     = profile.Id,
                    employee_id = profile.EmployeeId,
                    full_name   = profile.FullName,
                    report_type = reportType,
                    message     = message,
                    is_read     = false
                };

                var json    = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Post,
                    $"{SUPABASE_URL}/rest/v1/reports");
                request.Headers.Add("Authorization", $"Bearer {CurrentSession?.AccessToken}");
                request.Headers.Add("apikey", SUPABASE_ANON_KEY);
                request.Headers.Add("Prefer", "return=minimal");
                request.Content = content;

                var response = await _http.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    var err = await response.Content.ReadAsStringAsync();
                    return (false, err);
                }
                return (true, "");
            }
            catch (Exception ex) { return (false, ex.Message); }
        }

        // ── FETCH ALL REPORTS (Admin) ─────────────────────────────────────────────────
        public async Task<List<ReportModel>> FetchAllReportsAsync()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get,
                    $"{SUPABASE_URL}/rest/v1/reports?select=*&order=created_at.desc");
                request.Headers.Add("Authorization", $"Bearer {CurrentSession?.AccessToken}");
                request.Headers.Add("apikey", SUPABASE_ANON_KEY);

                var response = await _http.SendAsync(request);
                var body     = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode) return new List<ReportModel>();
                return JsonSerializer.Deserialize<List<ReportModel>>(body) ?? new List<ReportModel>();
            }
            catch { return new List<ReportModel>(); }
        }

        // ── MARK REPORT AS READ ───────────────────────────────────────────────────────
        public async Task<bool> MarkReportReadAsync(string reportId)
        {
            try
            {
                var payload = new { is_read = true };
                var json    = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Patch,
                    $"{SUPABASE_URL}/rest/v1/reports?id=eq.{reportId}");
                request.Headers.Add("Authorization", $"Bearer {CurrentSession?.AccessToken}");
                request.Headers.Add("apikey", SUPABASE_ANON_KEY);
                request.Headers.Add("Prefer", "return=minimal");
                request.Content = content;

                var response = await _http.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        // ── DELETE REPORT ─────────────────────────────────────────────────────────────
        public async Task<bool> DeleteReportAsync(string reportId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Delete,
                    $"{SUPABASE_URL}/rest/v1/reports?id=eq.{reportId}");
                request.Headers.Add("Authorization", $"Bearer {CurrentSession?.AccessToken}");
                request.Headers.Add("apikey", SUPABASE_ANON_KEY);

                var response = await _http.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        // ── DELETE EMPLOYEE ───────────────────────────────────────────────────────────
        public async Task<(bool success, string error)> DeleteEmployeeAsync(string userId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Delete,
                    $"{SUPABASE_URL}/rest/v1/users?id=eq.{userId}");
                request.Headers.Add("Authorization", $"Bearer {CurrentSession?.AccessToken}");
                request.Headers.Add("apikey", SUPABASE_ANON_KEY);

                var response = await _http.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    return (false, body);
                }
                return (true, "");
            }
            catch (Exception ex) { return (false, ex.Message); }
        }
    }
}