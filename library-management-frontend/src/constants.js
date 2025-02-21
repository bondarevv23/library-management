export const config = {
    apiUrl: process.env.REACT_APP_API_URL,
};

export const INITIAL_AUTHOR_FORM = { name: '', dateOfBirth: '' };
export const INITIAL_BOOK_FORM = { title: '', publicationYear: '', authorId: '' };

export const PAGE_SIZES = {
  DEFAULT: 10,
  LARGE: 100,
};