import React from 'react';
import { useNavigate } from "react-router-dom";
import Button from "react-bootstrap/Button";
import { ListGroup } from 'react-bootstrap';

export default function ChatItem(props) {
  const navigate = useNavigate();
  const chat = props.chat;
  const user1 = chat.user1.username;
  const user2 = chat.user2.username;

  console.log("chat: ", chat);

  const handleClick = (chat) => {
    navigate("/messages", {
      state: {
        chat: chat
      }
    })
  }

  return (
    <ListGroup.Item variant="light">
      <Button onClick={ () => handleClick(chat)} variant="primary" size="sm">
        ChatId: {chat.id} <br />
        Chat Name: {chat.name} <br />
        Participants: {user1}, {user2}
      </Button>
    </ListGroup.Item>
  )
}
