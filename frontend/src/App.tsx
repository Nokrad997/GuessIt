import React from 'react';
import './App.css';
import { Route, Routes } from 'react-router-dom';
import { LoadScriptNext } from '@react-google-maps/api'; 
import LoginPage from './pages/Login/Login';
import RegisterPage from './pages/Register/Register';
import HomePage from './pages/Home/Home';
import CustomNavbar from './components/Navbar/Navbar';
import Profile from './pages/Profile/Profile';
import Location from './pages/Location/Location';
import Game from './pages/Game/Game';
import UserAchievements from './pages/Achievements/Achievements';
import LeaderboardAndUserStats from './pages/LeaderboardAndUserStats/LeaderBoardAndUserStats';


function App() {
  return (
    <LoadScriptNext
      googleMapsApiKey={'AIzaSyB2mbuR4LbvGpVT5uAoftZbQkb72hhfY1g'} 
      libraries={['places', 'geometry']}
    >
      <div className="App">
        <div className="Navbar">
          <CustomNavbar />
        </div>
        <div className="Content">
          <Routes>
            <Route path="/" element={<HomePage />} />
            <Route path="/login" element={<LoginPage />} />
            <Route path="/register" element={<RegisterPage />} />
            <Route path="/profile/management" element={<Profile />} />
            <Route path='/profile/achievements' element={<UserAchievements />} />
            <Route path='/profile/statistics-leaderboard' element={<LeaderboardAndUserStats/>} />
            <Route path="/game-dashboard" element={<Location />} />
            <Route path="/game-dashboard/game" element={<Game />} />
          </Routes>
        </div>
      </div>
    </LoadScriptNext>
  );
}

export default App;
