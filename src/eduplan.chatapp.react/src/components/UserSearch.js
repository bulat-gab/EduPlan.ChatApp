import React, { useState } from 'react';
import { Form, Button } from 'react-bootstrap';
import { API_HOST } from './auth/ApiAuthorizationConstants';
import axios from "axios";
import AuthService from '../services/auth.service';
import { useNavigate } from "react-router-dom";

export default function UserSearch() {
  const [searchInput, setSearchInput] = useState("");
  const navigate = useNavigate();


  const handleSearchInput = event => {
    event.preventDefault();
    setSearchInput(event.target.value);
  }

  const handleSubmit = async (event) => {
    event.preventDefault();

    try{
      const response = await axios.post(
        `${API_HOST}api/v1/chat/?userId=${searchInput}`, {}, {
          headers: AuthService.authHeader()
        });
      console.log('Create chat: ', response);
      console.log(`Created chatId: ${response.data.id}`);

      navigate("/messages", {
        state: {
          chat: response.data
        }
      })

    } catch (err) {
      console.log(err)
    }
  }

  return (
    <Form onSubmit={handleSubmit}>
      <Form.Group className="mb-3" controlId="formUserId">
        <Form.Label>Username</Form.Label>
        <Form.Control
          type="text"
          placeholder="Enter user id"
          onChange={handleSearchInput} />
      </Form.Group>
      <Button variant="primary" type="submit">
        Submit
      </Button>
    </Form>
  )
}