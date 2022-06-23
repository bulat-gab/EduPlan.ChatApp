import React, { useEffect, useState } from 'react';
import AuthService from '../services/auth.service';
import axios from "axios";
import { API_HOST } from './auth/ApiAuthorizationConstants';
import { ListGroup } from 'react-bootstrap';
import ChatItem from './ChatItem';

export default function ChatList() {
  const [chats, setChats] = useState([]);

  const getChats = async () => {
    try {
      const response = await axios.get(`${API_HOST}api/v1/chat`, {
        headers: AuthService.authHeader()
      });
      setChats(response.data);
      console.log(response);
    }
    catch (err){
      console.error(err);
    }
  }

  useEffect(() => {
    getChats();
  }, []);

  return (
    <div>
      <h2>Chats page</h2>
      <ListGroup variant="flush">
        {chats.map(chat => <ChatItem key={chat.id} chat={chat}/>)}
      </ListGroup>
    </div>
  )
}
