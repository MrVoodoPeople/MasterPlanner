--
-- PostgreSQL database dump
--

-- Dumped from database version 16.2 (Debian 16.2-1.pgdg120+2)
-- Dumped by pg_dump version 16.2 (Debian 16.2-1.pgdg120+2)

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: datenotes; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.datenotes (
    id integer NOT NULL,
    date date,
    notes text
);


ALTER TABLE public.datenotes OWNER TO postgres;

--
-- Name: datenotes_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.datenotes_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.datenotes_id_seq OWNER TO postgres;

--
-- Name: datenotes_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.datenotes_id_seq OWNED BY public.datenotes.id;


--
-- Name: datenotes id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.datenotes ALTER COLUMN id SET DEFAULT nextval('public.datenotes_id_seq'::regclass);


--
-- Data for Name: datenotes; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.datenotes (id, date, notes) FROM stdin;
1	2024-03-16	Тест
\.


--
-- Name: datenotes_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.datenotes_id_seq', 1, true);


--
-- Name: datenotes datenotes_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.datenotes
    ADD CONSTRAINT datenotes_pkey PRIMARY KEY (id);


--
-- PostgreSQL database dump complete
--

