import { Link } from 'react-router-dom';
import React from 'react'
import { Button } from 'react-bootstrap';


export default function NavbarPublic() {
  return (
    <nav style={{ textAlign: "center", marginTop: "20px" }}>
      <Link to="/">
        Home
      </Link>
      <Link to="/about" style={{ padding: "10px" }}>
        About
      </Link>
      <Link to="login" style={{ padding: "10px" }}>
        Login
      </Link>

    </nav>
  )
}
