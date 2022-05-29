import { Link } from 'react-router-dom';

import React from 'react'

export default function Navbar() {
  return (
    <nav style={{ textAlign: "center", marginTop: "20px" }}>
      <Link to="/">
        Home
      </Link>
      <Link to="/profile" style={{ padding: "10px" }}>
        Profile
      </Link>
      <Link to="/messages" style={{ padding: "10px" }}>
        Messages
      </Link>
      <Link to="/search" style={{ padding: "10px" }}>
        Search
      </Link>
      <Link to="/about" style={{ padding: "10px" }}>
        About
      </Link>
    </nav>
  )
}
