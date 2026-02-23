import axios from "axios";

const API_BASE = "https://localhost:7019/api";
const STUDENT_URL = `${API_BASE}/Student`;
const LOGIN_URL = `${API_BASE}/Login`;

// Set default headers for all axios requests
axios.defaults.headers.common["Content-Type"] = "application/json";
axios.defaults.headers.common["Accept"] = "application/json";

// Set token directly in axios headers
export const setToken = (token) => {
  if (token) {
    axios.defaults.headers.common["Authorization"] = `Bearer ${token}`;
  } else {
    delete axios.defaults.headers.common["Authorization"];
  }
};

export const getToken = () => {
  return (
    axios.defaults.headers.common["Authorization"]?.replace("Bearer ", "") ||
    null
  );
};

export const login = async (policy, username, password) => {
  const response = await axios.post(LOGIN_URL, { policy, username, password });
  return response.data;
};

export const getAllStudents = async () => {
  const response = await axios.get(`${STUDENT_URL}/All`);

  //for preflight 
  // const response1 = await axios.delete(`${STUDENT_URL}/All`);
  return response.data;
};

// export const callMicrosoft1 = async () => {
//   const response = await axios.get(`${API_BASE}/Microsoft`);
//   return response.data;
// };
