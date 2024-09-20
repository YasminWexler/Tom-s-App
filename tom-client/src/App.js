import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Lobby from './components/Lobby/Lobby';
import CodeBlockPage from './components/CodeBlocks/CodeBlockPage'; 

function App() {
  return (
    <Router>
      <Routes>
       
        <Route path="/" element={<Lobby />} />
        <Route path="/codeblock/:name" element={<CodeBlockPage />} />
      </Routes>
    </Router>
  );
}

export default App;
