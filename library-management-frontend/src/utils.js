import axios from 'axios';
import { config } from './constants';

export const fetchData = async ({
  endpoint,
  params = {},
  setLoading,
  setError,
  setData,
  errorMessage,
}) => {
  setLoading(true);
  setError(null);
  try {
    const { data } = await axios.get(`${config.apiUrl}${endpoint}`, { params });
    setData(data);
    return data;
  } catch (error) {
    setError(errorMessage);
    console.error(`Error fetching ${endpoint}:`, error);
  } finally {
    setLoading(false);
  }
};

export const saveData = async ({
  endpoint,
  method,
  data,
  setLoading,
  setError,
  errorMessage,
}) => {
  setLoading(true);
  setError(null);
  try {
    const response = method === 'put'
      ? await axios.put(`${config.apiUrl}${endpoint}`, data)
      : await axios.post(`${config.apiUrl}${endpoint}`, data);
    return response.data;
  } catch (error) {
    setError(errorMessage);
    if (error.response?.data?.errors) throw error.response.data.errors;
    console.error(`Error saving ${endpoint}:`, error);
  } finally {
    setLoading(false);
  }
};

export const deleteData = async ({
  endpoint,
  setLoading,
  setError,
  errorMessage,
}) => {
  setLoading(true);
  setError(null);
  try {
    await axios.delete(`${config.apiUrl}${endpoint}`);
  } catch (error) {
    setError(errorMessage);
    console.error(`Error deleting ${endpoint}:`, error);
  } finally {
    setLoading(false);
  }
};

export const resetFormState = (setFormData, initialData, setEditingId, setError) => {
  setFormData(initialData);
  if (setEditingId) setEditingId(null);
  setError(null);
};