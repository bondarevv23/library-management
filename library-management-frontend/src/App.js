// src/App.js
import React from 'react';
import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import Authors from './components/Authors';
import Books from './components/Books';
import AuthorDetail from './components/AuthorDetail';
import BookDetail from './components/BookDetail';
import './App.css';

function App() {
  return (
    <Router>
      <div className="App">
        <nav>
          <Link to="/authors">Authors</Link>
          <Link to="/books">Books</Link>
        </nav>
        <Routes>
          <Route path="/" element={<Authors />} />
          <Route path="/authors" element={<Authors />} />
          <Route path="authors/:id" element={<AuthorDetail />} />
          <Route path="authors/new" element={<AuthorDetail />} />
          <Route path="books" element={<Books />} />
        <Route path="books/:id" element={<BookDetail />} />
        <Route path="books/new" element={<BookDetail />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;