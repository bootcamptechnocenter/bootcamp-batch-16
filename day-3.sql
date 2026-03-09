CREATE TABLE mst_brands(
	id serial primary key,
	code varchar(10) not null,
	name varchar(100) not null,
	is_active bool not null default true,
	created_at timestamp not null default now(),
	created_by varchar(100),
	updated_at timestamp,
	updated_by varchar(100),
	deleted_at timestamp ,
	deleted_by varchar(100)
);

insert into mst_brands (code, name, created_by) 
values 
	('HON', 'Honda', 'Admin'),
	('TOY', 'Toyota', 'Admin'),
	('BMW', 'BMW', 'Admin'),
	('SUZ', 'Suzuki', 'Admin'),
	('DAI', 'Daihatsu', 'Admin'),
	('MER', 'Mercedes-Benz', 'Admin');

create table mst_type (
	id serial primary key,
	brand_id integer not null references mst_brands(id),
	code varchar(10) not null,
	name varchar(100) not null,
	is_active bool not null default true,
	created_at timestamp not null default now(),
	created_by varchar(100),
	updated_at timestamp,
	updated_by varchar(100),
	deleted_at timestamp,
	deleted_by varchar(100)
);

insert into mst_type (brand_id, code, name, created_by)
values 
	(1, 'JAZZ', 'Honda Jazz', 'Admin'),
	(1, 'CRV', 'Honda Crv', 'Admin'),
	(2, 'MX', 'Toyota Mark X', 'Admin'),
	(3, 'Serie 3', '330i', 'Admin'),
	(6, 'C-Class', 'C300', 'Admin');

create table mst_models (
	id serial primary key,
	type_id integer not null references mst_type(id),
	code varchar(100),
	name varchar(100),
	year integer,
	created_at timestamp not null default now(),
	created_by varchar(100),
	updated_at timestamp,
	updated_by varchar(100),
	deleted_at timestamp,
	deleted_by varchar(100)
)

insert into mst_models (type_id, code, name, year, created_by)
values
	(5, 'W204', 'Facelift', 2013, 'Admin'),
	(5, 'W204', 'Pre-Facelift', 208, 'Admin'),
	(5, 'W205', '-', 2015, 'Admin');

create or replace procedure insert_type (
	in p_brand_id integer,
	in p_code varchar,
	in p_name varchar,
	in p_created_by varchar,
	out out_stat boolean,
	out out_mess text
)
language plpgsql
as $$
declare
v_brand_name varchar(100);
begin
	SELECT name INTO v_brand_name 
    FROM mst_brands 
    WHERE id = p_brand_id AND deleted_at IS NULL;

	if not exists (
		select 1 from mst_brands
		where p_brand_id = id and deleted_at is null
	) then
		out_stat := false;
		out_mess := 'Brand ID ' || p_brand_id || ' tidak ditemukan atau sudah dihapus';
	return;
	end if;

	if exists (
		select 1 from mst_type
		where p_code = code and p_brand_id = brand_id and deleted_at is null
	) then
		out_stat := false;
		out_mess := 'Kode sudah dipakai: ' || p_code;
	return;
	end if;

	insert into mst_type (brand_id, code, name, created_by)
	values (p_brand_id, p_code, p_name, 'Admin');
	
	out_stat := true;
	out_mess := 'Data berhasil ditambahkan';
end;
$$

call insert_type(6, 'E-Class', 'E55', 'ADMIN', null, null);
call insert_type(3, 'Serie 5', '530i', 'ADMIN', null, null);
call insert_type(8, 'Discovery', '-', 'ADMIN', null, null);