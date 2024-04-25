--
-- PostgreSQL database dump
--

-- Dumped from database version 16.2
-- Dumped by pg_dump version 16.2

-- Started on 2024-04-25 12:41:40

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

--
-- TOC entry 4 (class 2615 OID 2200)
-- Name: public; Type: SCHEMA; Schema: -; Owner: pg_database_owner
--

CREATE SCHEMA public;


ALTER SCHEMA public OWNER TO pg_database_owner;

--
-- TOC entry 4910 (class 0 OID 0)
-- Dependencies: 4
-- Name: SCHEMA public; Type: COMMENT; Schema: -; Owner: pg_database_owner
--

COMMENT ON SCHEMA public IS 'standard public schema';


--
-- TOC entry 232 (class 1255 OID 16598)
-- Name: delete_book_author(integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.delete_book_author(p_book_author_id integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    DELETE FROM public."BookAuthor"
    WHERE "BookAuthorId" = p_book_author_id;
END;
$$;


ALTER FUNCTION public.delete_book_author(p_book_author_id integer) OWNER TO postgres;

--
-- TOC entry 238 (class 1255 OID 16604)
-- Name: delete_cart_session(integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.delete_cart_session(p_cart_session_id integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    DELETE FROM public."CartSession"
    WHERE "CartSessionId" = p_cart_session_id;
END;
$$;


ALTER FUNCTION public.delete_cart_session(p_cart_session_id integer) OWNER TO postgres;

--
-- TOC entry 235 (class 1255 OID 16601)
-- Name: delete_cart_session_detail(integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.delete_cart_session_detail(p_cart_session_detail_id integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    DELETE FROM public."CartSessionDetalle"
    WHERE "CartSessionDetailId" = p_cart_session_detail_id;
END;
$$;


ALTER FUNCTION public.delete_cart_session_detail(p_cart_session_detail_id integer) OWNER TO postgres;

--
-- TOC entry 229 (class 1255 OID 16595)
-- Name: deleteacademicgrade(integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.deleteacademicgrade(p_academicgradeid integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    DELETE FROM public."AcademicGrade"
    WHERE AcademicGradeId = p_AcademicGradeId;
END;
$$;


ALTER FUNCTION public.deleteacademicgrade(p_academicgradeid integer) OWNER TO postgres;

--
-- TOC entry 226 (class 1255 OID 16591)
-- Name: deletebook(uuid); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.deletebook(p_bookid uuid) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    DELETE FROM public."Book"
    WHERE "BookId" = p_BookId;
END;
$$;


ALTER FUNCTION public.deletebook(p_bookid uuid) OWNER TO postgres;

--
-- TOC entry 239 (class 1255 OID 16605)
-- Name: get_academic_grade_list(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.get_academic_grade_list() RETURNS TABLE(academicgradeid integer, academicgradeguid text, name text, academiccenter text, fechagrado timestamp with time zone, bookauthorid integer)
    LANGUAGE plpgsql
    AS $$
BEGIN
    RETURN QUERY SELECT * FROM public."AcademicGrade";
END;
$$;


ALTER FUNCTION public.get_academic_grade_list() OWNER TO postgres;

--
-- TOC entry 240 (class 1255 OID 16607)
-- Name: get_book_author_list(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.get_book_author_list() RETURNS TABLE(bookauthorid integer, bookauthorguid text, name text, lastname text, birthdate timestamp with time zone)
    LANGUAGE plpgsql
    AS $$
BEGIN
    RETURN QUERY SELECT * FROM public."BookAuthor";
END;
$$;


ALTER FUNCTION public.get_book_author_list() OWNER TO postgres;

--
-- TOC entry 259 (class 1255 OID 16630)
-- Name: get_book_info(uuid); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.get_book_info(p_bookid uuid) RETURNS TABLE(bookid uuid, title text, filedirection text, publicationdate timestamp with time zone, bookauthorguid text, authorname text, authorlastname text, authorbirthdate timestamp with time zone)
    LANGUAGE plpgsql
    AS $$
BEGIN
    RETURN QUERY SELECT "b"."BookId", 
                          "b"."Title", 
                          "b"."FileDirection", 
                          "b"."PublicationDate", 
                          "ba"."BookAuthorGuid", 
                          "ba"."Name" AS "AuthorName", 
                          "ba"."LastName" AS "AuthorLastName", 
                          "ba"."Birthdate" AS "AuthorBirthdate"
                    FROM public."Book" "b"
                    LEFT JOIN public."BookAuthor" "ba" ON "b"."BookAuthorGuid" = "ba"."BookAuthorGuid"
                    WHERE "b"."BookId" = p_BookId;
END;
$$;


ALTER FUNCTION public.get_book_info(p_bookid uuid) OWNER TO postgres;

--
-- TOC entry 257 (class 1255 OID 16626)
-- Name: get_book_list(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.get_book_list() RETURNS TABLE(bookid uuid, title text, filedirection text, publicationdate timestamp with time zone, name text, lastname text, bookauthorguid text, fileextension text, filename text)
    LANGUAGE plpgsql
    AS $$
BEGIN
    RETURN QUERY 
    SELECT b."BookId", b."Title", b."FileDirection", b."PublicationDate", 
           COALESCE(a."Name", ''), COALESCE(a."LastName", ''), b."BookAuthorGuid", 
           b."FileExtension", b."FileName"
    FROM public."Book" b
    LEFT JOIN public."BookAuthor" a ON b."BookAuthorGuid" = a."BookAuthorGuid";
END;
$$;


ALTER FUNCTION public.get_book_list() OWNER TO postgres;

--
-- TOC entry 242 (class 1255 OID 16609)
-- Name: get_cart_session_detalle_list(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.get_cart_session_detalle_list() RETURNS TABLE(cartsessiondetailid integer, creationdate timestamp with time zone, selectedproduct text, cartsessionid integer)
    LANGUAGE plpgsql
    AS $$
BEGIN
    RETURN QUERY SELECT * FROM public."CartSessionDetalle";
END;
$$;


ALTER FUNCTION public.get_cart_session_detalle_list() OWNER TO postgres;

--
-- TOC entry 241 (class 1255 OID 16608)
-- Name: get_cart_session_list(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.get_cart_session_list() RETURNS TABLE(cartsessionid integer, creationdate timestamp with time zone)
    LANGUAGE plpgsql
    AS $$
BEGIN
    RETURN QUERY SELECT * FROM public."CartSession";
END;
$$;


ALTER FUNCTION public.get_cart_session_list() OWNER TO postgres;

--
-- TOC entry 227 (class 1255 OID 16593)
-- Name: getacademicgradebyid(integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.getacademicgradebyid(p_academicgradeid integer) RETURNS TABLE(academicgradeid integer, academicgradeguid text, name text, academiccenter text, fechagrado timestamp without time zone, bookauthorid integer)
    LANGUAGE plpgsql
    AS $$
BEGIN
    RETURN QUERY SELECT AcademicGradeId, AcademicGradeGuid, Name, AcademicCenter, FechaGrado, BookAuthorId FROM public."AcademicGrade" WHERE AcademicGradeId = p_AcademicGradeId;
END;
$$;


ALTER FUNCTION public.getacademicgradebyid(p_academicgradeid integer) OWNER TO postgres;

--
-- TOC entry 255 (class 1255 OID 16615)
-- Name: getacademicgradesbyauthor(integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.getacademicgradesbyauthor(p_bookauthorid integer) RETURNS TABLE(academicgradeid integer, academicgradeguid text, name text, academiccenter text, fechagrado timestamp with time zone)
    LANGUAGE plpgsql
    AS $$
BEGIN
    RETURN QUERY 
    SELECT AcademicGradeId, AcademicGradeGuid, Name, AcademicCenter, FechaGrado
    FROM public."AcademicGrade"
    WHERE BookAuthorId = p_BookAuthorId;
END;
$$;


ALTER FUNCTION public.getacademicgradesbyauthor(p_bookauthorid integer) OWNER TO postgres;

--
-- TOC entry 254 (class 1255 OID 16614)
-- Name: getauthorbyid(integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.getauthorbyid(p_bookauthorid integer) RETURNS TABLE(bookauthorid integer, bookauthorguid text, name text, lastname text, birthdate timestamp with time zone)
    LANGUAGE plpgsql
    AS $$
BEGIN
    RETURN QUERY 
    SELECT BookAuthorId, BookAuthorGuid, Name, LastName, Birthdate
    FROM public."BookAuthor"
    WHERE BookAuthorId = p_BookAuthorId;
END;
$$;


ALTER FUNCTION public.getauthorbyid(p_bookauthorid integer) OWNER TO postgres;

--
-- TOC entry 230 (class 1255 OID 16596)
-- Name: insert_book_author(text, text, text, timestamp with time zone); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.insert_book_author(p_book_author_guid text, p_name text, p_lastname text, p_birthdate timestamp with time zone) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    INSERT INTO public."BookAuthor" (
        "BookAuthorGuid",
        "Name",
        "LastName",
        "Birthdate"
    ) VALUES (
        p_book_author_guid,
        p_name,
        p_lastname,
        p_birthdate
    );
END;
$$;


ALTER FUNCTION public.insert_book_author(p_book_author_guid text, p_name text, p_lastname text, p_birthdate timestamp with time zone) OWNER TO postgres;

--
-- TOC entry 236 (class 1255 OID 16602)
-- Name: insert_cart_session(timestamp with time zone); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.insert_cart_session(p_creation_date timestamp with time zone) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    INSERT INTO public."CartSession" (
        "CreationDate"
    ) VALUES (
        p_creation_date
    );
END;
$$;


ALTER FUNCTION public.insert_cart_session(p_creation_date timestamp with time zone) OWNER TO postgres;

--
-- TOC entry 233 (class 1255 OID 16599)
-- Name: insert_cart_session_detail(timestamp with time zone, text, integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.insert_cart_session_detail(p_creation_date timestamp with time zone, p_selected_product text, p_cart_session_id integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    INSERT INTO public."CartSessionDetalle" (
        "CreationDate",
        "SelectedProduct",
        "CartSessionId"
    ) VALUES (
        p_creation_date,
        p_selected_product,
        p_cart_session_id
    );
END;
$$;


ALTER FUNCTION public.insert_cart_session_detail(p_creation_date timestamp with time zone, p_selected_product text, p_cart_session_id integer) OWNER TO postgres;

--
-- TOC entry 225 (class 1255 OID 16592)
-- Name: insertacademicgrade(text, text, text, timestamp without time zone, integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.insertacademicgrade(p_academicgradeguid text, p_name text, p_academiccenter text, p_fechagrado timestamp without time zone, p_bookauthorid integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    INSERT INTO public."AcademicGrade" (AcademicGradeGuid, Name, AcademicCenter, FechaGrado, BookAuthorId)
    VALUES (p_AcademicGradeGuid, p_Name, p_AcademicCenter, p_FechaGrado, p_BookAuthorId);
END;
$$;


ALTER FUNCTION public.insertacademicgrade(p_academicgradeguid text, p_name text, p_academiccenter text, p_fechagrado timestamp without time zone, p_bookauthorid integer) OWNER TO postgres;

--
-- TOC entry 258 (class 1255 OID 16620)
-- Name: insertbook(uuid, text, text, text, text, timestamp with time zone, text); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.insertbook(p_bookid uuid, p_title text, p_fileextension text, p_filename text, p_filedirection text, p_publicationdate timestamp with time zone, p_bookauthorguid text) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    INSERT INTO public."Book" ("BookId", "Title", "FileExtension", "FileName", "FileDirection", "PublicationDate", "BookAuthorGuid") 
    VALUES (p_BookId, p_Title, p_FileExtension, p_FileName, p_FileDirection, p_PublicationDate, p_BookAuthorGuid);
END;
$$;


ALTER FUNCTION public.insertbook(p_bookid uuid, p_title text, p_fileextension text, p_filename text, p_filedirection text, p_publicationdate timestamp with time zone, p_bookauthorguid text) OWNER TO postgres;

--
-- TOC entry 231 (class 1255 OID 16597)
-- Name: update_book_author(integer, text, text, text, timestamp with time zone); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.update_book_author(p_book_author_id integer, p_book_author_guid text, p_name text, p_lastname text, p_birthdate timestamp with time zone) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    UPDATE public."BookAuthor"
    SET "BookAuthorGuid" = p_book_author_guid,
        "Name" = p_name,
        "LastName" = p_lastname,
        "Birthdate" = p_birthdate
    WHERE "BookAuthorId" = p_book_author_id;
END;
$$;


ALTER FUNCTION public.update_book_author(p_book_author_id integer, p_book_author_guid text, p_name text, p_lastname text, p_birthdate timestamp with time zone) OWNER TO postgres;

--
-- TOC entry 237 (class 1255 OID 16603)
-- Name: update_cart_session(integer, timestamp with time zone); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.update_cart_session(p_cart_session_id integer, p_creation_date timestamp with time zone) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    UPDATE public."CartSession"
    SET "CreationDate" = p_creation_date
    WHERE "CartSessionId" = p_cart_session_id;
END;
$$;


ALTER FUNCTION public.update_cart_session(p_cart_session_id integer, p_creation_date timestamp with time zone) OWNER TO postgres;

--
-- TOC entry 234 (class 1255 OID 16600)
-- Name: update_cart_session_detail(integer, timestamp with time zone, text, integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.update_cart_session_detail(p_cart_session_detail_id integer, p_creation_date timestamp with time zone, p_selected_product text, p_cart_session_id integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    UPDATE public."CartSessionDetalle"
    SET "CreationDate" = p_creation_date,
        "SelectedProduct" = p_selected_product,
        "CartSessionId" = p_cart_session_id
    WHERE "CartSessionDetailId" = p_cart_session_detail_id;
END;
$$;


ALTER FUNCTION public.update_cart_session_detail(p_cart_session_detail_id integer, p_creation_date timestamp with time zone, p_selected_product text, p_cart_session_id integer) OWNER TO postgres;

--
-- TOC entry 228 (class 1255 OID 16594)
-- Name: updateacademicgrade(integer, text, text, text, timestamp without time zone, integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.updateacademicgrade(p_academicgradeid integer, p_academicgradeguid text, p_name text, p_academiccenter text, p_fechagrado timestamp without time zone, p_bookauthorid integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    UPDATE public."AcademicGrade"
    SET AcademicGradeGuid = p_AcademicGradeGuid, Name = p_Name, AcademicCenter = p_AcademicCenter, FechaGrado = p_FechaGrado, BookAuthorId = p_BookAuthorId
    WHERE AcademicGradeId = p_AcademicGradeId;
END;
$$;


ALTER FUNCTION public.updateacademicgrade(p_academicgradeid integer, p_academicgradeguid text, p_name text, p_academiccenter text, p_fechagrado timestamp without time zone, p_bookauthorid integer) OWNER TO postgres;

--
-- TOC entry 256 (class 1255 OID 16621)
-- Name: updatebook(uuid, text, text, text, text, timestamp with time zone, text); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.updatebook(p_bookid uuid, p_title text, p_fileextension text, p_filename text, p_filedirection text, p_publicationdate timestamp with time zone, p_bookauthorguid text) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    UPDATE public."Book"
    SET
        "Title" = p_Title,
        "FileExtension" = p_FileExtension,
        "FileName" = p_FileName,
        "FileDirection" = p_FileDirection,
        "PublicationDate" = p_PublicationDate,
        "BookAuthorGuid" = p_BookAuthorGuid
    WHERE
        "BookId" = p_BookId;
END;
$$;


ALTER FUNCTION public.updatebook(p_bookid uuid, p_title text, p_fileextension text, p_filename text, p_filedirection text, p_publicationdate timestamp with time zone, p_bookauthorguid text) OWNER TO postgres;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 222 (class 1259 OID 16561)
-- Name: AcademicGrade; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."AcademicGrade" (
    "AcademicGradeId" integer NOT NULL,
    "AcademicGradeGuid" text DEFAULT ''::text NOT NULL,
    "Name" text,
    "AcademicCenter" text,
    "FechaGrado" timestamp with time zone,
    "BookAuthorId" integer NOT NULL
);


ALTER TABLE public."AcademicGrade" OWNER TO postgres;

--
-- TOC entry 221 (class 1259 OID 16560)
-- Name: AcademicGrade_AcademicGradeId_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public."AcademicGrade" ALTER COLUMN "AcademicGradeId" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."AcademicGrade_AcademicGradeId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 216 (class 1259 OID 16539)
-- Name: Book; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Book" (
    "BookId" uuid NOT NULL,
    "Title" text NOT NULL,
    "FileDirection" text,
    "PublicationDate" timestamp with time zone,
    "BookAuthorGuid" text,
    "FileExtension" text,
    "FileName" text,
    "FileSize" text
);


ALTER TABLE public."Book" OWNER TO postgres;

--
-- TOC entry 218 (class 1259 OID 16547)
-- Name: BookAuthor; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."BookAuthor" (
    "BookAuthorId" integer NOT NULL,
    "BookAuthorGuid" text NOT NULL,
    "Name" text,
    "LastName" text,
    "Birthdate" timestamp with time zone
);


ALTER TABLE public."BookAuthor" OWNER TO postgres;

--
-- TOC entry 217 (class 1259 OID 16546)
-- Name: BookAuthor_BookAuthorId_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public."BookAuthor" ALTER COLUMN "BookAuthorId" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."BookAuthor_BookAuthorId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 220 (class 1259 OID 16555)
-- Name: CartSession; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."CartSession" (
    "CartSessionId" integer NOT NULL,
    "CreationDate" timestamp with time zone
);


ALTER TABLE public."CartSession" OWNER TO postgres;

--
-- TOC entry 224 (class 1259 OID 16574)
-- Name: CartSessionDetalle; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."CartSessionDetalle" (
    "CartSessionDetailId" integer NOT NULL,
    "CreationDate" timestamp with time zone,
    "SelectedProduct" text NOT NULL,
    "CartSessionId" integer NOT NULL
);


ALTER TABLE public."CartSessionDetalle" OWNER TO postgres;

--
-- TOC entry 223 (class 1259 OID 16573)
-- Name: CartSessionDetalle_CartSessionDetailId_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public."CartSessionDetalle" ALTER COLUMN "CartSessionDetailId" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."CartSessionDetalle_CartSessionDetailId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 219 (class 1259 OID 16554)
-- Name: CartSession_CartSessionId_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public."CartSession" ALTER COLUMN "CartSessionId" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."CartSession_CartSessionId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 215 (class 1259 OID 16534)
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres;

--
-- TOC entry 4902 (class 0 OID 16561)
-- Dependencies: 222
-- Data for Name: AcademicGrade; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."AcademicGrade" ("AcademicGradeId", "AcademicGradeGuid", "Name", "AcademicCenter", "FechaGrado", "BookAuthorId") FROM stdin;
\.


--
-- TOC entry 4896 (class 0 OID 16539)
-- Dependencies: 216
-- Data for Name: Book; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Book" ("BookId", "Title", "FileDirection", "PublicationDate", "BookAuthorGuid", "FileExtension", "FileName", "FileSize") FROM stdin;
c29819d1-fce2-4f4c-a279-2f8e14a47496	Libro 4 edit	\N	1998-03-04 00:00:00-06	5502769d-6b98-4ff7-a79f-68f055d7f371	\N	\N	\N
a1a1d78c-52d2-4777-857a-4cacc69787d1	Libro 2	\N	2020-01-02 00:00:00-06	e2b43dbc-2193-4566-9d6b-c741b9281824	\N	\N	\N
cdf3b31a-bbe1-45bb-9ab6-8e42b94a2e77	editado 3	\N	2024-04-24 00:00:00-05	35b85d4b-9dfc-4126-bfd4-14d9d65b5d1f	\N	\N	\N
d39d05d1-81a9-4707-a6a0-917c47dd9f4b	C# .Net	\N	0008-12-09 00:00:00-05:50:36	12d857cb-e26a-4c13-9c12-b350ff30d22a	\N	\N	\N
\.


--
-- TOC entry 4898 (class 0 OID 16547)
-- Dependencies: 218
-- Data for Name: BookAuthor; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."BookAuthor" ("BookAuthorId", "BookAuthorGuid", "Name", "LastName", "Birthdate") FROM stdin;
2	35b85d4b-9dfc-4126-bfd4-14d9d65b5d1f	Angel	Loredo	2024-04-24 00:00:00-05
3	12d857cb-e26a-4c13-9c12-b350ff30d22a	Pedro	Lopez	2024-04-25 00:00:00-05
4	59dad15f-f60c-4e00-8f62-66480b912d40	MIGUEL	HERRERA	2024-04-25 00:00:00-05
5	e2b43dbc-2193-4566-9d6b-c741b9281824	MARIA	ESQUIVEL	2024-04-25 00:00:00-05
6	5502769d-6b98-4ff7-a79f-68f055d7f371	MANUEL	JIMENEZ	2024-04-25 00:00:00-05
\.


--
-- TOC entry 4900 (class 0 OID 16555)
-- Dependencies: 220
-- Data for Name: CartSession; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."CartSession" ("CartSessionId", "CreationDate") FROM stdin;
\.


--
-- TOC entry 4904 (class 0 OID 16574)
-- Dependencies: 224
-- Data for Name: CartSessionDetalle; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."CartSessionDetalle" ("CartSessionDetailId", "CreationDate", "SelectedProduct", "CartSessionId") FROM stdin;
\.


--
-- TOC entry 4895 (class 0 OID 16534)
-- Dependencies: 215
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
20240424053148_InitialMigration	8.0.4
20240424161744_updateauthorid	8.0.4
20240424183318_updatebookmodel	8.0.4
20240424233902_updatebookmodeaddingsfilesize	8.0.4
\.


--
-- TOC entry 4911 (class 0 OID 0)
-- Dependencies: 221
-- Name: AcademicGrade_AcademicGradeId_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."AcademicGrade_AcademicGradeId_seq"', 1, false);


--
-- TOC entry 4912 (class 0 OID 0)
-- Dependencies: 217
-- Name: BookAuthor_BookAuthorId_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."BookAuthor_BookAuthorId_seq"', 6, true);


--
-- TOC entry 4913 (class 0 OID 0)
-- Dependencies: 223
-- Name: CartSessionDetalle_CartSessionDetailId_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."CartSessionDetalle_CartSessionDetailId_seq"', 1, false);


--
-- TOC entry 4914 (class 0 OID 0)
-- Dependencies: 219
-- Name: CartSession_CartSessionId_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."CartSession_CartSessionId_seq"', 1, false);


--
-- TOC entry 4746 (class 2606 OID 16567)
-- Name: AcademicGrade PK_AcademicGrade; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."AcademicGrade"
    ADD CONSTRAINT "PK_AcademicGrade" PRIMARY KEY ("AcademicGradeId");


--
-- TOC entry 4739 (class 2606 OID 16545)
-- Name: Book PK_Book; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Book"
    ADD CONSTRAINT "PK_Book" PRIMARY KEY ("BookId");


--
-- TOC entry 4741 (class 2606 OID 16553)
-- Name: BookAuthor PK_BookAuthor; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."BookAuthor"
    ADD CONSTRAINT "PK_BookAuthor" PRIMARY KEY ("BookAuthorId");


--
-- TOC entry 4743 (class 2606 OID 16559)
-- Name: CartSession PK_CartSession; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."CartSession"
    ADD CONSTRAINT "PK_CartSession" PRIMARY KEY ("CartSessionId");


--
-- TOC entry 4749 (class 2606 OID 16580)
-- Name: CartSessionDetalle PK_CartSessionDetalle; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."CartSessionDetalle"
    ADD CONSTRAINT "PK_CartSessionDetalle" PRIMARY KEY ("CartSessionDetailId");


--
-- TOC entry 4737 (class 2606 OID 16538)
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- TOC entry 4744 (class 1259 OID 16586)
-- Name: IX_AcademicGrade_BookAuthorId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_AcademicGrade_BookAuthorId" ON public."AcademicGrade" USING btree ("BookAuthorId");


--
-- TOC entry 4747 (class 1259 OID 16587)
-- Name: IX_CartSessionDetalle_CartSessionId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_CartSessionDetalle_CartSessionId" ON public."CartSessionDetalle" USING btree ("CartSessionId");


--
-- TOC entry 4750 (class 2606 OID 16568)
-- Name: AcademicGrade FK_AcademicGrade_BookAuthor_BookAuthorId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."AcademicGrade"
    ADD CONSTRAINT "FK_AcademicGrade_BookAuthor_BookAuthorId" FOREIGN KEY ("BookAuthorId") REFERENCES public."BookAuthor"("BookAuthorId") ON DELETE CASCADE;


--
-- TOC entry 4751 (class 2606 OID 16581)
-- Name: CartSessionDetalle FK_CartSessionDetalle_CartSession_CartSessionId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."CartSessionDetalle"
    ADD CONSTRAINT "FK_CartSessionDetalle_CartSession_CartSessionId" FOREIGN KEY ("CartSessionId") REFERENCES public."CartSession"("CartSessionId") ON DELETE CASCADE;


-- Completed on 2024-04-25 12:41:40

--
-- PostgreSQL database dump complete
--

