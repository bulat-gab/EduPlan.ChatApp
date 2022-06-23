import { Link } from 'react-router-dom';
import React from 'react'
import Nav from 'react-bootstrap/Nav';

export default function NavbarPublic() {
  return (
    <>
      <Nav.Link as={Link} to="/">Home</Nav.Link>
      <Nav.Link as={Link} to="/about">About</Nav.Link>
      <Nav.Link as={Link} to="/login">Login</Nav.Link>
    </>
  )
}
