--latihan 2 procedure
create or replace procedure insert_type(
	in p_brand_id integer,
	in p_code varchar,
	in p_name varchar,
	in p_created_by varchar,
	out out_stat boolean,
	out out_mess text
)
language plpgsql
as $$
begin
	if not exists (
		select 1 from mst_brands
		where id = p_brand_id and deleted_at is null
	) then
		out_stat := false;
		out_mess := 'brand_id belum ada atau sudah dihapus di mst_brands'; 
		return;
	end if;

	if exists (
		select 1 from mst_types
		where code = p_code and brand_id = p_brand_id and deleted_at is null
	) then
		out_stat := false;
		out_mess := 'code dan brand_id sudah ada di mst_types: ' || p_code;
		return;
	end if;

	insert into mst_types(brand_id, code, name, created_by, created_at)
	values (p_brand_id, p_code, p_name, p_created_by, now());
	
	out_stat := true;
	out_mess := 'data berhasil ditambahkan';
end;
$$;

call insert_type(3, 'JIMNY', 'Jimny', 'Admin', null, null);
call insert_type(6, 'ERT', 'Ertiga', 'Admin', null, null);
call insert_type(3, 'JIMNY', 'Jimny', 'Admin', null, null);
call insert_type(3, 'JIMNY', 'Jimny', 'Admin', null, null);