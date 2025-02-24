import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import '../styles.css';
import { INITIAL_AUTHOR_FORM } from '../constants';
import { fetchData, saveData, deleteData } from '../utils';

function AuthorDetail() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [author, setAuthor] = useState(null);
  const [books, setBooks] = useState([]);
  const [formData, setFormData] = useState(INITIAL_AUTHOR_FORM);
  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);
  const [serverError, setServerError] = useState(null);
  const [isEditing, setIsEditing] = useState(!id);

  const validateForm = () => {
    const newErrors = {};
    const today = new Date();

    if (!formData.name.trim()) newErrors.name = 'Name is required';
    else if (formData.name.length < 3) newErrors.name = 'Name must be at least 3 characters long';
    else if (formData.name.length > 255) newErrors.name = 'Name cannot exceed 255 characters';

    if (!formData.dateOfBirth) newErrors.dateOfBirth = 'Date of Birth is required';
    else {
      const dob = new Date(formData.dateOfBirth);
      if (dob > today) newErrors.dateOfBirth = 'Date of Birth cannot be in the future';
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  useEffect(() => {
    if (id) {
      fetchAuthor();
      fetchBooksByAuthor();
    } else {
      setFormData(INITIAL_AUTHOR_FORM);
      setAuthor(null);
      setBooks([]);
      setIsEditing(true);
    }
  }, [id]);

  const fetchAuthor = useCallback(() => {
    return fetchData({
      endpoint: `/authors/${id}`,
      setLoading,
      setError: setServerError,
      setData: (data) => {
        setAuthor(data);
        setFormData({ name: data.name, dateOfBirth: data.dateOfBirth.split('T')[0] });
      },
      errorMessage: 'Failed to fetch author details',
    });
  }, [id]);
  
  const fetchBooksByAuthor = useCallback(() => {
    return fetchData({
      endpoint: `/authors/${id}/books`,
      setLoading,
      setError: setServerError,
      setData: (data) => setBooks(data || []),
      errorMessage: 'Failed to fetch books by author',
    });
  }, [id]);

  const handleInputChange = ({ target: { name, value } }) => {
    setFormData((prev) => ({ ...prev, [name]: value }));
    if (errors[name]) setErrors((prev) => ({ ...prev, [name]: '' }));
  };

  const handleSave = async (e) => {
    e.preventDefault();
    if (!validateForm()) return;

    try {
      const data = await saveData({
        endpoint: id ? `/authors/${id}` : '/authors',
        method: id ? 'put' : 'post',
        data: formData,
        setLoading,
        setError: setServerError,
        errorMessage: `Failed to ${id ? 'update' : 'create'} author`,
      });
      if (id) {
        setAuthor((prev) => ({ ...prev, ...formData }));
        setIsEditing(false);
      } else {
        navigate(`/authors/${data.id}`);
      }
    } catch (serverErrors) {
      if (serverErrors) setErrors(serverErrors);
    }
  };

  const handleDelete = () => {
    if (!id || !window.confirm('Are you sure you want to delete this author?')) return;
    deleteData({
      endpoint: `/authors/${id}`,
      setLoading,
      setError: setServerError,
      errorMessage: 'Failed to delete author',
    }).then(() => navigate('/authors'));
  };

  const handleEditToggle = () => setIsEditing(true);

  const handleCancel = () => {
    if (id) {
      setFormData({ name: author.name, dateOfBirth: author.dateOfBirth.split('T')[0] });
      setIsEditing(false);
    } else {
      navigate('/authors');
    }
    setErrors({});
    setServerError(null);
  };

  const handleBack = () => navigate('/authors');
  const handleBookClick = (bookId) => navigate(`/books/${bookId}`);

  if (loading) return <div>Loading...</div>;

  const renderForm = () => (
    <form onSubmit={handleSave}>
      <div className="input-group">
        <label>Name:</label>
        <input
          type="text"
          name="name"
          value={formData.name}
          onChange={handleInputChange}
          disabled={loading}
          className={errors.name ? 'error' : ''}
        />
        {errors.name && <span className="error-message">{errors.name}</span>}
      </div>
      <div className="input-group">
        <label>Date of Birth:</label>
        <input
          type="date"
          name="dateOfBirth"
          value={formData.dateOfBirth}
          onChange={handleInputChange}
          disabled={loading}
          className={errors.dateOfBirth ? 'error' : ''}
        />
        {errors.dateOfBirth && <span className="error-message">{errors.dateOfBirth}</span>}
      </div>
      <div className="button-group">
        <button type="submit" disabled={loading} className="button button-save">{id ? 'Update' : 'Create'}</button>
        <button type="button" onClick={handleCancel} disabled={loading} className="button button-cancel">Cancel</button>
        {id && <button type="button" onClick={handleDelete} disabled={loading} className="button button-delete">Delete</button>}
      </div>
    </form>
  );

  const renderDetails = () => (
    <>
      {author ? (
        <>
          <p><strong>ID:</strong> {author.id}</p>
          <p><strong>Name:</strong> {author.name}</p>
          <p><strong>Date of Birth:</strong> {author.dateOfBirth}</p>
          <div className="button-group">
            <button onClick={handleEditToggle} disabled={loading} className="button button-edit">Edit</button>
            <button onClick={handleDelete} disabled={loading} className="button button-delete">Delete</button>
            <button onClick={handleBack} disabled={loading} className="button button-cancel">Back to Authors</button>
          </div>
          <h3>Books by {author.name}</h3>
          {books.length > 0 ? (
            <table className="table">
              <thead>
                <tr>
                  <th>ID</th>
                  <th>Title</th>
                  <th>Publication Year</th>
                </tr>
              </thead>
              <tbody>
                {books.map((book) => (
                  <tr key={book.id}>
                    <td onClick={() => handleBookClick(book.id)} className="clickable">{book.id}</td>
                    <td>{book.title}</td>
                    <td>{book.publicationYear}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          ) : (
            <p>No books found for this author.</p>
          )}
        </>
      ) : (
        <p>Author not found</p>
      )}
    </>
  );

  return (
    <div className="author-detail container">
      <h2>{id ? (isEditing ? 'Edit Author' : 'Author Details') : 'Create New Author'}</h2>
      {serverError && <div className="error">{serverError}</div>}
      {isEditing ? renderForm() : renderDetails()}
    </div>
  );
}

export default AuthorDetail;