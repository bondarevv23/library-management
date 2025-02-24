CREATE TABLE authors
(
    id            BIGSERIAL    NOT NULL PRIMARY KEY,
    name          VARCHAR(255) NOT NULL,
    date_of_birth DATE         NOT NULL
);

CREATE TABLE books
(
    id               BIGSERIAL    NOT NULL PRIMARY KEY,
    title            VARCHAR(255) NOT NULL,
    publication_year INT          NOT NULL,
    author_id        BIGINT       NOT NULL
        CONSTRAINT fk_author_id REFERENCES authors ON DELETE CASCADE
);
