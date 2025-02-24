import React, { useState, useEffect, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import '../styles.css'; 
import { PAGE_SIZES } from '../constants';
import { deleteData, fetchData } from '../utils';

function Authors() {
  const navigate = useNavigate();
  const [authors, setAuthors] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [pageNumber, setPageNumber] = useState(1);
  const [totalPages, setTotalPages] = useState(1);

  const fetchAuthors = useCallback(() => {
      return fetchData({
        endpoint: '/authors',
        params: { pageNumber, pageSize: PAGE_SIZES.DEFAULT },
        setLoading,
        setError,
        setData: (data) => {
          setAuthors(data.data);
          setPageNumber(data.pageNumber);
          setTotalPages(data.totalPages);
        },
        errorMessage: 'Failed to fetch authors',
      });
  }, [pageNumber, setLoading, setError, setAuthors, setPageNumber, setTotalPages]);

  useEffect(() => {
    fetchAuthors();
  }, [pageNumber, fetchAuthors]);

  const handleDelete = (id) => {
    if (!window.confirm('Are you sure you want to delete this author?')) return;
    deleteData({
      endpoint: `/authors/${id}`,
      setLoading,
      setError,
      errorMessage: 'Failed to delete author',
    }).then(() => fetchAuthors());
  };

  const handleEdit = (author) => navigate(`/authors/${author.id}`);
  const handleAuthorClick = (id) => navigate(`/authors/${id}`);
  const handleCreateNew = () => navigate('/authors/new');

  const handlePrevious = () => pageNumber > 1 && setPageNumber((prev) => prev - 1);
  const handleNext = () => pageNumber < totalPages && setPageNumber((prev) => prev + 1);

  const renderTable = () => (
    <table className="table">
      <thead>
        <tr>
          <th>ID</th>
          <th>Name</th>
          <th>Date of Birth</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        {authors.map((author) => (
          <tr key={author.id}>
            <td onClick={() => handleAuthorClick(author.id)} className="clickable">{author.id}</td>
            <td>{author.name}</td>
            <td>{author.dateOfBirth.split('T')[0]}</td>
            <td>
              <button onClick={() => handleEdit(author)} disabled={loading} className="button button-edit">Edit</button>
              <button onClick={() => handleDelete(author.id)} disabled={loading} className="button button-delete">Delete</button>
            </td>
          </tr>
        ))}
      </tbody>
    </table>
  );

  const renderPagination = () => (
    <div className="pagination">
      <button onClick={handlePrevious} disabled={loading || pageNumber === 1} className="paginate-button">Previous</button>
      <span>Page {pageNumber} of {totalPages || 1}</span>
      <button onClick={handleNext} disabled={loading || pageNumber === totalPages} className="next-button">Next</button>
    </div>
  );

  return (
    <div className="authors">
      <h2>Authors Management</h2>
      <button onClick={handleCreateNew} className="create-button">Create New Author</button>
      {error && <div className="error">{error}</div>}
      {loading && <div>Loading...</div>}
      {authors && authors.length > 0 ? (
        <>
          {renderTable()}
          {renderPagination()}
        </>
      ) : (
        <p>No authors available</p>
      )}
    </div>
  );
}

export default Authors;