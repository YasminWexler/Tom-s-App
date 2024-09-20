import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { Editor } from "@monaco-editor/react";
import { io } from "socket.io-client";
import axios from "axios";
import s from "./Style.module.css";

const socket = io("https://localhost:7109/codeblockHub");

function CodeBlockPage() {

  const { name } = useParams();
  const navigate = useNavigate();
  const [code, setCode] = useState(""); 
  const [solutionCode, setSolutionCode] = useState(""); 
  const [role, setRole] = useState("student"); 
  const [studentCount, setStudentCount] = useState(0); 
  const [smileyVisible, setSmileyVisible] = useState(false); 

  useEffect(() => {
    axios
      .get(`https://localhost:7109/getinitialCode/${name}`)
      .then((response) => {
        setCode(response.data.initialCode);
        setSolutionCode(response.data.solutionCode);
      })
      .catch((error) => {
        console.error("Error fetching code block", error);
      });
  
    socket.emit("joinRoom", name);
  
    socket.on("roleAssigned", (assignedRole) => {
      console.log("Role assigned:", assignedRole);
      setRole(assignedRole);
    });
  
    socket.on("studentCountUpdate", (count) => {
      console.log("Student count updated:", count);
      setStudentCount(count);
    });
  
    socket.on("receiveCodeChange", (newCode) => {
      setCode(newCode);
      socket.off();
    });

    socket.on("RedirectToLobby", () => {
        navigate("/"); 
      });

    return () => {
      socket.emit("leaveRoom", name);
    };
    
  }, [name]);

  const handleCodeChange = (newCode) => {
    setCode(newCode);
    socket.emit("codeChanged", name, newCode);
  
    if (newCode === solutionCode) {
      setSmileyVisible(true);
    } else {
      setSmileyVisible(false); 
    }
  };
  

  return (
    <div className={s.root}>
      <h1>{name}</h1>
      <div className={s.infoContainer}>
        <p className={s.role}>Your role: {role}</p>
        <p className={s.number}>Number of students: {studentCount}</p>
      </div>
      <Editor
        height="400px"
        language="javascript"
        theme="vs-dark"
        value={code}
        onChange={(newCode) => handleCodeChange(newCode)}
        options={{ readOnly: role === "mentor" }} 
      />
      {smileyVisible && <div>😊</div>} {}
      <div className={s.button_container}>
        <button className={s.back_button} onClick={() => navigate("/")}>
          Back
        </button>
      </div>
    </div>
  );
}

export default CodeBlockPage;
