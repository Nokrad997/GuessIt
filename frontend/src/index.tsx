import React from 'react';
import ReactDOM from 'react-dom/client';
import 'bootstrap/dist/css/bootstrap.min.css';
import './index.css';
import App from './App';
import { BrowserRouter } from 'react-router-dom';
import { ErrorProvider } from './components/ErrorContext/ErrorContext';

const root = ReactDOM.createRoot(document.getElementById('root') as HTMLElement);
root.render(
  <React.StrictMode>
    <ErrorProvider>
      <BrowserRouter>
        <App />
      </BrowserRouter>
    </ErrorProvider>
  </React.StrictMode>
);