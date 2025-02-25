import React, { useState, useEffect, useCallback } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import '../styles.css';
import { INITIAL_BOOK_FORM, PAGE_SIZES } from '../constants';
import { fetchData, saveData, deleteData } from '../utils';

function BookDetail() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [book, setBook] = useState(null);
  const [authors, setAuthors] = useState([]);
  const [formData, setFormData] = useState(INITIAL_BOOK_FORM);
  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);
  const [serverError, setServerError] = useState(null);
  const [isEditing, setIsEditing] = useState(!id);

  const validateForm = () => {
    const newErrors = {};
    const pubYear = parseInt(formData.publicationYear, 10);
    const currentYear = new Date().getFullYear();

    if (!formData.title.trim()) newErrors.title = 'Title is required';
    else if (formData.title.length === 0) newErrors.title = 'Title must be at least 1 characters long';
    else if (formData.title.length > 255) newErrors.title = 'Title cannot exceed 255 characters';

    if (!formData.publicationYear) newErrors.publicationYear = 'Publication Year is required';
    else if (isNaN(pubYear)) newErrors.publicationYear = 'Publication Year must be a valid number';
    else if (pubYear <= 0) newErrors.publicationYear = `Year must be positive`;
    else if (pubYear > currentYear) newErrors.publicationYear = `Year must not be in future`;

    if (!formData.authorId) newErrors.authorId = 'Please select an author';

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const fetchBook = useCallback(() => {
    return fetchData({
      endpoint: `/books/${id}`,
      setLoading,
      setError: setServerError,
      setData: (data) => {
        setBook(data);
        setFormData({
          title: data.title,
          publicationYear: data.publicationYear.toString(),
          authorId: data.authorId,
        });
      },
      errorMessage: 'Failed to fetch book details',
    });
  }, [id, setLoading, setServerError, setBook, setFormData])

  const fetchAllAuthors = useCallback(() => {
    return fetchData({
      endpoint: '/authors',
      params: { pageSize: PAGE_SIZES.LARGE },
      setLoading,
      setError: setServerError,
      setData: (data) => setAuthors(data.data || []),
      errorMessage: 'Failed to fetch authors',
    });
  }, [setLoading, setServerError, setAuthors])

  useEffect(() => {
    fetchAllAuthors();
    if (id){
      fetchBook();
    } else {
      setFormData(INITIAL_BOOK_FORM);
      setBook(null);
      setIsEditing(true);
    }
  }, [id, fetchAllAuthors, fetchBook]);

  const handleInputChange = ({ target: { name, value } }) => {
    setFormData((prev) => ({ ...prev, [name]: value }));
    if (errors[name]) setErrors((prev) => ({ ...prev, [name]: '' }));
  };

  const handleSave = async (e) => {
    e.preventDefault();
    if (!validateForm()) return;

    const submissionData = { ...formData, publicationYear: parseInt(formData.publicationYear, 10) };

    try {
      const data = await saveData({
        endpoint: id ? `/books/${id}` : '/books',
        method: id ? 'put' : 'post',
        data: submissionData,
        setLoading,
        setError: setServerError,
        errorMessage: `Failed to ${id ? 'update' : 'create'} book`,
      });
      if (id) {
        setBook((prev) => ({ ...prev, ...submissionData }));
        setIsEditing(false);
      } else {
        navigate(`/books/${data.id}`);
      }
    } catch (serverErrors) {
      if (serverErrors) setErrors(serverErrors);
    }
  };

  const handleDelete = () => {
    if (!id || !window.confirm('Are you sure you want to delete this book?')) return;
    deleteData({
      endpoint: `/books/${id}`,
      setLoading,
      setError: setServerError,
      errorMessage: 'Failed to delete book',
    }).then(() => navigate('/books'));
  };

  const handleEditToggle = () => setIsEditing(true);

  const handleCancel = () => {
    if (id) {
      setFormData({ title: book.title, publicationYear: book.publicationYear.toString(), authorId: book.authorId });
      setIsEditing(false);
    } else {
      navigate('/books');
    }
    setErrors({});
    setServerError(null);
  };

  const handleCreate = () => {
    setBook(null);
    setAuthors([]);
    setFormData(INITIAL_BOOK_FORM);
    setErrors({});
    setLoading(false);
    setServerError(null);
    navigate('/books/new');
  };

  const handleBack = () => navigate('/books');

  if (loading) return <div>Loading...</div>;

  const renderForm = () => (
    <form onSubmit={handleSave}>
      <div className="input-group">
        <label>Title:</label>
        <input
          type="text"
          name="title"
          value={formData.title}
          onChange={handleInputChange}
          disabled={loading}
          className={errors.title ? 'error' : ''}
        />
        {errors.title && <span className="error-message">{errors.title}</span>}
      </div>
      <div className="input-group">
        <label>Publication Year:</label>
        <input
          type="number"
          name="publicationYear"
          value={formData.publicationYear}
          onChange={handleInputChange}
          disabled={loading}
          className={errors.publicationYear ? 'error' : ''}
        />
        {errors.publicationYear && <span className="error-message">{errors.publicationYear}</span>}
      </div>
      <div className="input-group">
        <label>Author:</label>
        <select
          name="authorId"
          value={formData.authorId}
          onChange={handleInputChange}
          disabled={loading || authors.length === 0}
          className={errors.authorId ? 'error' : ''}
        >
          <option value="">Select an author</option>
          {authors.map((author) => (
            <option key={author.id} value={author.id}>{author.name} (ID: {author.id})</option>
          ))}
        </select>
        {errors.authorId && <span className="error-message">{errors.authorId}</span>}
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
      {book ? (
        <>
          <p><strong>ID:</strong> {book.id}</p>
          <p><strong>Title:</strong> {book.title}</p>
          <p><strong>Publication Year:</strong> {book.publicationYear}</p>
          <p><strong>Author:</strong> {authors.find((a) => a.id === book.authorId)?.name || 'Unknown'}</p>
          <div className="button-group">
            <button onClick={handleCreate} disabled={loading} className="button button-save">Create</button>
            <button onClick={handleEditToggle} disabled={loading} className="button button-edit">Edit</button>
            <button onClick={handleDelete} disabled={loading} className="button button-delete">Delete</button>
            <button onClick={handleBack} disabled={loading} className="button button-cancel">Back to Books</button>
          </div>
        </>
      ) : (
        <p>Book not found</p>
      )}
    </>
  );

  return (
    <div className="book-detail container">
      <h2>{id ? (isEditing ? 'Edit Book' : 'Book Details') : 'Create New Book'}</h2>
      {serverError && <div className="error">{serverError}</div>}
      {isEditing ? renderForm() : renderDetails()}
    </div>
  );
}

export default BookDetail;