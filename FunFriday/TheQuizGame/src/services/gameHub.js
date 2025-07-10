// src/services/gameHub.js
import * as signalR from "@microsoft/signalr";

let connection = null;

export const startConnection = async () => {
  connection = new signalR.HubConnectionBuilder()
    .withUrl("https://h34858mr-44300.inc1.devtunnels.ms/gamehub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

  try {
    await connection.start();
    console.log("SignalR Connected.");
  } catch (err) {
    console.log(err);
    setTimeout(startConnection, 5000);
  }

  return connection;
};

export const getConnection = () => connection;
