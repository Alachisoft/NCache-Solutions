-- Table: public.customers

-- DROP TABLE public.customers;

CREATE TABLE public.customers
(
    customerid text COLLATE pg_catalog."default" NOT NULL,
    address text COLLATE pg_catalog."default" NOT NULL,
    city text COLLATE pg_catalog."default" NOT NULL,
    country text COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT pk_constraint PRIMARY KEY (customerid)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public.customers
    OWNER to postgres;

COMMENT ON TABLE public.customers
    IS 'customers table to use with NCache demo';