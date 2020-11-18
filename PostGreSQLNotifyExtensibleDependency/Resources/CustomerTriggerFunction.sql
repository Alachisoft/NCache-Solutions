-- FUNCTION: public.notifyoncustomerdatachange()

-- DROP FUNCTION public.notifyoncustomerdatachange();

CREATE FUNCTION public.notifyoncustomerdatachange()
    RETURNS trigger
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE NOT LEAKPROOF
AS $BODY$
DECLARE
	channel text := TG_ARGV[0];
	notification JSON;
	dep_key text;
BEGIN
	if (TG_OP = 'DELETE') THEN
		dep_key = OLD.customerid;
	ELSE
		dep_key = NEW.customerid;
	END IF;
	
	notification = json_build_object(
		'table', TG_TABLE_NAME,
		'dep_key', dep_key,
		'schema', TG_TABLE_SCHEMA);
		
	PERFORM pg_notify(channel, notification::TEXT);
	RETURN NEW;
END
$BODY$;

ALTER FUNCTION public.notifyoncustomerdatachange()
    OWNER TO postgres;

COMMENT ON FUNCTION public.notifyoncustomerdatachange()
    IS 'NCache notifications on customers table from PostGreSQL';
