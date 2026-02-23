import StudentList from "./components/StudentList";
import Login from "./components/Login";
import "./index.css";

function App() {
  return (
    <div className="container">
      <h2>Student API Test UI</h2>
      <Login />
      <hr className="divider" />
      <StudentList />
    </div>
  );
}

export default App;
