import React from 'react';
import { ListGroup } from 'react-bootstrap';

export default function MessageItem(props) {
  const message = props.message;

  let formattedDate = new Date(message.createdAt + "Z");
  formattedDate = formattedDate.toLocaleString();

  return (
    <ListGroup.Item
    key={message.id}
    variant={message.fromId == "1" ? "primary" : "success"}
  >
    Date: {formattedDate} <br/>
    FromUserId: {message.fromId} <br/>
    Text: {message.text}
  </ListGroup.Item>
  )
}