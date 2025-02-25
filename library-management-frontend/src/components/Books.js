import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { PAGE_SIZES } from '../constants';
import '../styles.css';
import { deleteData, fetchData } from '../utils';

function Books() {
  const navigate = useNavigate();
  const [books, setBooks] = useState({ data: [], totalPages: 1 });
  const [authors, setAuthors] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [pageNumber, setPageNumber] = useState(1);
  const [searchQuery, setSearchQuery] = useState('');
  const [isSearching, setIsSearching] = useState(false);

  useEffect(() => {
    fetchBooks(pageNumber);
    fetchAllAuthors();
  }, [pageNumber]);

  const fetchBooks = (page) => fetchData({
    endpoint: '/books',
    params: { pageNumber: page, pageSize: PAGE_SIZES.DEFAULT },
    setLoading,
    setError,
    setData: (data) => {
      setIsSearching(false);
      setBooks({ data: data.data || [], totalPages: data.totalPages || 1 });
      console.log('Fetched books for page', page, data);
    },
    errorMessage: 'Failed to fetch books',
  });

  const fetchBooksBySearch = (query) => fetchData({
    endpoint: '/books/search',
    params: { query: encodeURIComponent(query) },
    setLoading,
    setError,
    setData: (data) => {
      setIsSearching(true);
      setBooks({ data: data.data || [], totalPages: 1 });
    },
    errorMessage: 'Failed to fetch search results',
  });

  const fetchAllAuthors = () => fetchData({
    endpoint: '/authors',
    params: { pageSize: PAGE_SIZES.LARGE },
    setLoading,
    setError,
    setData: (data) => setAuthors(data.data || []),
    errorMessage: 'Failed to fetch authors',
  });

  const handleSearchChange = ({ target: { value } }) => setSearchQuery(value);

  const handleSearchSubmit = (e) => {
    if (e.key === 'Enter') {
      e.preventDefault();
      if (searchQuery.trim()) fetchBooksBySearch(searchQuery);
      else fetchBooks(pageNumber);
      setPageNumber(1);
    }
  };

  const handleDelete = (id) => {
    if (!window.confirm('Are you sure you want to delete this book?')) return;
    deleteData({
      endpoint: `/books/${id}`,
      setLoading,
      setError,
      errorMessage: 'Failed to delete book',
    }).then(() => fetchBooks());;
  };

  const handleEdit = (book) => navigate(`/books/${book.id}`);
  const handleBookClick = (id) => navigate(`/books/${id}`);
  const handleAuthorClick = (authorId) => navigate(`/authors/${authorId}`);
  const handleCreateNew = () => navigate('/books/new');

  const handlePrevious = () => pageNumber > 1 && !loading && setPageNumber((prev) => prev - 1);
  const handleNext = () => pageNumber < books.totalPages && !loading && setPageNumber((prev) => prev + 1);

  const renderSearch = () => (
    <div className="search-group">
      <label>Search Books:</label>
      <input
        type="text"
        value={searchQuery}
        onChange={handleSearchChange}
        onKeyDown={handleSearchSubmit}
        placeholder="Search query..."
        disabled={loading}
        className="search-input"
      />
    </div>
  );

  const renderTable = () => (
    <table className="table">
      <thead>
        <tr>
          <th>ID</th>
          <th>Title</th>
          <th>Publication Year</th>
          <th>Author</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        {books.data.map((book) => (
          <tr key={book.id}>
            <td onClick={() => handleBookClick(book.id)} className="clickable">{book.id}</td>
            <td>{book.title}</td>
            <td>{book.publicationYear}</td>
            <td onClick={() => handleAuthorClick(book.authorId)} className="clickable">
              {authors.find((a) => a.id === book.authorId)?.name || 'Unknown'}
            </td>
            <td>
              <button onClick={() => handleEdit(book)} disabled={loading} className="button button-edit">Edit</button>
              <button onClick={() => handleDelete(book.id)} disabled={loading} className="button button-delete">Delete</button>
            </td>
          </tr>
        ))}
      </tbody>
    </table>
  );

  const renderPagination = () => (
    <div className="pagination">
      <button onClick={handlePrevious} disabled={loading || pageNumber === 1} className="paginate-button">Previous</button>
      <span>Page {pageNumber} of {books.totalPages || 1}</span>
      <button onClick={handleNext} disabled={loading || pageNumber === books.totalPages} className="next-button">Next</button>
    </div>
  );

  return (
    <div className="books">
      <h2>Books Management</h2>
      <button onClick={handleCreateNew} className="create-button">Create New Book</button>
      {renderSearch()}
      {error && <div className="error">{error}</div>}
      {loading && <div>Loading...</div>}
      {books.data && books.data.length > 0 ? (
        <>
          {renderTable()}
          {!isSearching && renderPagination()}
        </>
      ) : (
        <p>{searchQuery && isSearching ? 'No search results found' : 'No books available'}</p>
      )}
    </div>
  );
}

export default Books;