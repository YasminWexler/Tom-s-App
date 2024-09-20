import React from 'react';
import { useNavigate } from 'react-router-dom';
import s from './Style.module.css';

export function Lobby() {
  const navigate = useNavigate();

  const handleButtonClick = (name) => {
    navigate(`/codeblock/${name}`); 
  };

  return (
    <div className={s.root}>
      <div>
        <h1>Choose code block</h1>
      </div>
      <div className={s.btn_container}>
        <button className={s.btn_lobby} onClick={() => handleButtonClick('Async-Case')}>
          Async case
        </button>
        <button className={s.btn_lobby} onClick={() => handleButtonClick('Callback-Hell')}>
          Callback Hell
        </button>
        <button className={s.btn_lobby} onClick={() => handleButtonClick('Promises-Puzzle')}>
          Promises Puzzle
        </button>
        <button className={s.btn_lobby} onClick={() => handleButtonClick('Event-Loop-Explained')}>
          Event Loop Explained
        </button>
        <button className={s.btn_lobby} onClick={() => handleButtonClick('Array-Methods-Mastery')}>
          Array Methods Mastery
        </button>
      </div>
    </div>
  );
}

export default Lobby;
