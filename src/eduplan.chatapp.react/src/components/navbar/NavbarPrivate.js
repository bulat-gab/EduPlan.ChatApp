import { Link } from 'react-router-dom';
import React from 'react'
import Nav from 'react-bootstrap/Nav';

export default function NavbarPrivate() {
  return (
    <>
      <Nav.Link as={Link} to="/">Home</Nav.Link>
      <Nav.Link as={Link} to="/profile">Profile</Nav.Link>
      <Nav.Link as={Link} to="/chats">Chats</Nav.Link>
      <Nav.Link as={Link} to="/search">Search</Nav.Link>
      <Nav.Link as={Link} to="/about">About</Nav.Link>
      <Nav.Link as={Link} to="/logout">Logout</Nav.Link>
    </>
  )
}
