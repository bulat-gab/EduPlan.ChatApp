import NavbarPrivate from './NavbarPrivate';
import NavbarPublic from './NavbarPublic';
import React from 'react'
import AuthService from '../services/auth.service';
import Container from 'react-bootstrap/Container';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
import NavDropdown from 'react-bootstrap/NavDropdown';

export default function NavMenu() {
  const isAuthenticated = AuthService.isAuthenticated();  

  return (
    <Navbar bg="light" expand="lg">
      <Container>
      <Navbar.Brand href="/">Chat App</Navbar.Brand>
      <Nav className="me-auto">
        {isAuthenticated ? <NavbarPrivate /> : <NavbarPublic />}
      </Nav>
      </Container>
    </Navbar>
  )
}
