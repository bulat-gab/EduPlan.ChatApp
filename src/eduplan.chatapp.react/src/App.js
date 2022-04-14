import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import './App.css';
import { Button } from 'react-bootstrap';
import NavMenu from './components/NavMenu';
import Messages from "./components/Messages";
import Home from "./components/Home";

const jwt = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxIiwiZW1haWwiOiJnYWJkcmFraG1hbm92LmJyQGdtYWlsLmNvbSIsIm5hbWUiOiJnYWJkcmFraG1hbm92LmJyQGdtYWlsLmNvbSIsIm5iZiI6MTY0OTkxODg0MSwiZXhwIjoxNjUyNTEwODQxLCJpYXQiOjE2NDk5MTg4NDEsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjUxMTEiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo1MTExIn0.ra4zljByYAETKMnPkY9mvopDfmbzYGfgt6yflhFGR94";

function App() {
  return (
    <div className="App">
      <h1>Chat App</h1>
      <Router>
        <Routes>
          <Route path="/" element={<Home />}/>
          <Route path="/messages" element={<Messages token={jwt} />}/>
        </Routes>
      </Router>
      
    </div>
  );
}

export default App;
