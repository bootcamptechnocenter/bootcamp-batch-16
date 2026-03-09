--create mst_brand
create table mst_brands(
	id serial primary key,
	code varchar(10) not null,
	name varchar(100) not null,
	is_active boolean not null default true,
	created_at timestamp not null default now(),
	created_by varchar(100),
	updated_at timestamp,
	updated_by varchar(100),
	deleted_at timestamp,
	deleted_by varchar(100)
);

--insert brand
insert into mst_brands(code, name, created_by)
values('HON', 'Honda', 'Admin')

--bulk insert
insert into mst_brands(code, name, created_by)
values
	('TOY', 'TOYOTA', 'Admin'),
	('SUZ', 'SUZUKI', 'Admin'),
	('DAI', 'DAIHATSU', 'Admin');

--select table
select * from mst_brands;

--select yang belum dihapus
select * from mst_brands where deleted_at is null;

--select + filter + sort
select id, name, is_active
from mst_brands where deleted_at is null and is_active = true
order by name asc;

--pencairan case-insensitive
select * from mst_brands
where deleted_at is null and name ilike '%toy%';

--update
update mst_brands
set name = 'Honda Motor Indonesia', updated_at = now(), updated_by = 'Admin'
where id = 3 and deleted_at is null;

--delete (soft delete)
update mst_brands
set deleted_at = now(), deleted_by = 'Admin', is_active = false where id = 4

--create table mst_type
create table mst_types(
	id serial primary key,
	brand_id integer not null references mst_brands(id),
	code varchar(10) not null,
	name varchar(100) not null,
	is_active boolean not null default true,
	created_at timestamp not null default now(),
	created_by varchar(100),
	updated_at timestamp,
	updated_by varchar(100),
	deleted_at timestamp,
	deleted_by varchar(100)
);

--select table
select * from mst_types;

-- INSERT DATA mst_types
insert into mst_types(brand_id, code, name, created_by)
values
	(3, 'JAZZ', 'Honda Jazz', 'Admin'),
	(3, 'CRV', 'Honda CRV', 'Admin'),
	(4, 'MX', 'Toyota Mark X', 'Admin');

-- select join
select
	t.id as type_id,
	t.code as type_code,
	t.name as type_name,
	b.name as brand_name
from mst_types t
join mst_brands b on t.brand_id = b.id
where t.deleted_at is null
and b.deleted_at is null
order by b.name, t.name;

--create PRC
create or replace procedure insert_brand(
	in p_code varchar,
	in p_name varchar,
	in p_created_by varchar,
	out out_stat boolean,
	out out_mess text
)
language plpgsql
as $$
begin
	if exists(
		select 1 from mst_Brands
		where code = p_code and deleted_at is null
	) then
		out_stat := false;
		out_mess := 'Kode sudah dipakai: ' || p_code;
		return;
	end if;

	insert into mst_brands(code, name, created_by, created_at)
	values (p_code, p_name, p_created_by, now());
	
	out_stat := true;
	out_mess := 'Data berhasil ditambahkan';
end;
$$;

call insert_brand('MZD', 'Mazda', 'Admin', null, null);

--PRC soft delete
-- prosedur dibawah lebih bagus pakai id
create or replace procedure delete_brand(
	in p_code varchar,
	in p_deleted_by varchar,
	out out_stat boolean,
	out out_mess text
)
language plpgsql
as $$
begin
	if exists(
		select 1 from mst_brands
		where code = p_code and deleted_at is not null
	) then
		out_stat := false;
		out_mess := 'Kode sudah terhapus ' || p_code;
		return;
	end if;
	
	update mst_brands set deleted_at = now(), deleted_by = p_deleted_by, is_active = false where code = p_code;
	
	out_stat := true;
	out_mess := 'Kode berhasil dihapus';
end;
$$;

call delete_brand('MZD', 'Admin', null, null);
call delete_brand('SUZ', 'Danu', null, null);

--create view dengan join
create or replace view v_types_with_brand as
select
	t.id	as type_id,
	t.name	as type_name,
	t.code	as type_code,
	b.id	as brand_id,
	b.name	as brand_name
from mst_types t
join mst_brands b on t.brand_id = b.id
where t.deleted_at is null
and b.deleted_at is null;

select * from v_types_with_brand;

--latihan 1
-- create table mst_models
create table mst_models(
	id serial primary key,
	type_id integer not null references mst_types(id),
	code varchar(10) not null,
	name varchar(100) not null,
	year integer not null,
	is_active boolean not null default true,
	created_at timestamp not null default now(),
	created_by varchar(100),
	updated_at timestamp,
	updated_by varchar(100),
	deleted_at timestamp,
	deleted_by varchar(100)
);

-- insert data mst_models
insert into mst_models(type_id, code, name, year, created_by)
values
	(4, 'MSN', 'Jazz MSN', 2026, 'Admin'),
	(5, 'BBC', 'CRV BBC', 2025, 'Admin'),
	(6, 'IDN', 'MARK X IDN', 2024, 'Admin');

-- select join 3 table (mst_models -> mst_types -> mst_brands)
select
	m.id as model_id,
	m.code as model_code,
	m.name as model_name,
	m.year as model_year,
	t.name as type_name,
	b.name as brand_name
from mst_models m
join mst_types t on m.type_id = t.id
join mst_brands b on t.brand_id = b.id
where m.deleted_at is null
  and t.deleted_at is null
  and b.deleted_at is null;

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

call insert_type(3, 'HRV', 'Honda HRV', 'Admin', null, null);
call insert_type(6, 'XEN', 'Daihatsu Xenia', 'Admin', null, null);
call insert_type(3, 'HRV', 'Honda HRV Prestige', 'Admin', null, null);