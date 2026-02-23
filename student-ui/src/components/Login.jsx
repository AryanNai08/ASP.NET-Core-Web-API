import { useState } from "react";
import { login, setToken } from "../api/studentApi";

function Login({ onLoginSuccess }) {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [policy, setPolicy] = useState("Local");
  const [loggedInUser, setLoggedInUser] = useState(null);
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  const handleLogin = async () => {
    // if (!username || !password) {
    //   setError("Please provide username & password");
    //   return;
    // }

    setLoading(true);
    setError("");

    try {
      const response = await login(policy, username, password);
      console.log("Login response:", response);

      if (response && response.token) {
        // Store token in header for future requests
        setToken(response.token);
        setLoggedInUser(response.username);
        setError("");

        // Notify parent component
        if (onLoginSuccess) {
          onLoginSuccess(response.username, response.token);
        }
      } else {
        // Handle case where API returns string message for invalid credentials
        const errorMsg =
          typeof response === "string"
            ? response
            : "Invalid username & password";
        setError(errorMsg);
      }
    } catch (err) {
      console.error("Login error:", err);
      // Properly extract error message
      let errorMsg = "Login failed";
      if (err.response?.data) {
        errorMsg =
          typeof err.response.data === "string"
            ? err.response.data
            : JSON.stringify(err.response.data);
      } else if (err.message) {
        errorMsg = err.message;
      }
      setError(errorMsg);
    } finally {
      setLoading(false);
    }
  };

  const handleLogout = () => {
    setToken(null);
    setLoggedInUser(null);
    setUsername("");
    setPassword("");
  };

  return (
    <div className="login-container">
      {loggedInUser ? (
        <div className="logged-in">
          <span className="welcome-text">
            Welcome, <strong>{loggedInUser}</strong>!
          </span>
          <button onClick={handleLogout} className="logout-btn">
            Logout
          </button>
        </div>
      ) : (
        <div className="login-form">
          <input
            type="text"
            placeholder="Username"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            className="login-input"
          />
          <input
            type="password"
            placeholder="Password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            className="login-input"
          />
          <select
            value={policy}
            onChange={(e) => setPolicy(e.target.value)}
            className="login-input"
          >
            <option value="Local">Local</option>
            <option value="Microsoft">Microsoft</option>
            <option value="Google">Google</option>
          </select>
          <button
            onClick={handleLogin}
            disabled={loading}
            className="login-btn"
          >
            {loading ? "Logging in..." : "Login"}
          </button>
        </div>
      )}
      {error && <p className="error">{error}</p>}
    </div>
  );
}

export default Login;
