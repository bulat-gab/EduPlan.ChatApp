import React, { useState, useEffect } from 'react';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import './App.css';
import Messages from './components/Messages';
import Profile from './components/Profile';
import UserSearch from './components/UserSearch';
import Navbar from './components/Navbar';
import LoginMenu from './components/auth/LoginMenu';
import Home from './components/Home';
import About from './components/About';
import PrivateRoute from './components/auth/PrivateRoute';
import { Container, Row, Button } from 'react-bootstrap';
import AuthService from './services/auth.service';
import Logout from './components/auth/Logout';
import ChatList from './components/ChatList';

const App = () => {
  const [user, setUser] = useState(null);

  useEffect(() => {
    const signedUser = AuthService.getUser();
    if (signedUser !== null) {
      setUser(signedUser);
    }

  }, [])

  const handleUserLogin = (accessToken) => {
    const result = AuthService.signin(accessToken);
    console.log(`Authentication result: ${result}`);

    const signedUser = AuthService.getUser();
    if (!signedUser) {
      setUser(signedUser);
    }
  }

  const handleSignout = () => {
    console.log(`${user.name} has signed out.`);
    AuthService.logout();
    setUser(null);
  }

  return (
    <div className='App'>
      <h1>Chat App</h1>
      <BrowserRouter>
        <Navbar />
        <Routes>
          <Route element={ <PrivateRoute /> } >
            <Route path="/profile" element={ <Profile /> }/>
            <Route path="/messages" element={ <Messages /> }/>
            <Route path="/chats" element={ <ChatList /> }/>
            <Route path="/search" element={ <UserSearch /> }/>
            <Route path="/logout" element={ <Logout /> }/>
          </Route>
          <Route path="/" element={ <Home /> }/>
          <Route path="/about" element={ <About /> }/>
          <Route path="/login" element={ <LoginMenu onUserLogin={handleUserLogin} /> }/>
          <Route path="/login-callback" element={ <LoginMenu onUserLogin={handleUserLogin} /> }/>
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
