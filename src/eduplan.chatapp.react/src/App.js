import React, { useState, useEffect } from 'react';
import './App.css';
import Messages from './components/Messages';
import LoginMenu from './components/auth/LoginMenu';
import { Container, Row, Button } from 'react-bootstrap';
import AuthService from './services/auth.service';

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
    AuthService.signout();
    setUser(null);
  }

  return (
    <div className='App'>
      <h1>Chat App</h1>
      <Button onClick={handleSignout}>Signout</Button>
      {user ? <Messages /> : <LoginMenu onUserLogin={handleUserLogin}/>}
    </div>
  );
}

export default App;
