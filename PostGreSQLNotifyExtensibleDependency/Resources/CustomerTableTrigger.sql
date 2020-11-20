-- Trigger: ncachecustomertrigger

-- DROP TRIGGER ncachecustomertrigger ON public.customers;

CREATE TRIGGER ncachecustomertrigger
    AFTER DELETE OR UPDATE 
    ON public.customers
    FOR EACH ROW
    EXECUTE PROCEDURE public.notifyoncustomerdatachange('customer_channel');

COMMENT ON TRIGGER ncachecustomertrigger ON public.customers
    IS 'Trigger to invoke function that notifies on customer data change for use by NCache';