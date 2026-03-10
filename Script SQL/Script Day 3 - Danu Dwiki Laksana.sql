-- Create table mst_brands
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


-- Insert brand
insert into mst_brands (code, name, created_by)
values ('HON', 'HONDA', 'Admin');


-- Bulk insert
insert into mst_brands (code, name, created_by )
values 
	('TOY','TOYOTA', 'Admin'),
	('SUZ','SUZUKI', 'Admin'),
	('DAI','DAIHATSU', 'Admin');

-- Select all brands
select * from mst_brands mb;

-- Select yang belum dihapus
select * from mst_brands mb 
where mb.deleted_at is null;

-- Select + filter + sort
select mb.id, mb."name" , mb.is_active 
from mst_brands mb
where mb.deleted_at is null
and mb.is_active = true
order by mb."name" asc;


-- Search case insensitive
select * from mst_brands mb 
where deleted_at is null
and mb."name" ilike '%suzu%';


-- count 
select count(*) from mst_brands mb 
where mb.deleted_at is null;


-- update
update mst_brands
set name = 'Honda Motor Indonesia',
	updated_at = now(),
	updated_by = 'Admin'
where id = 1
and deleted_at  is null;
	

-- soft delete data
update mst_brands mb 
set deleted_at = now(),
	deleted_by = 'Admin',
	is_active = false
where id = 2;


-- create table mst_type
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

-- INSERT DATA mst_types
insert into mst_types(brand_id, code, name, created_by)
values
	(1, 'JAZZ', 'Honda Jazz', 'Admin'),
	(1, 'CRV', 'Honda CRV', 'Admin'),
	(2, 'MX', 'Toyota Mark X', 'Admin');

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
	

-- Create PRC dengan validasi duplikasi data
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
		select 1 from mst_brands
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


call insert_brand('MZS', 'MAZDA', 'Admin', null, null);
call insert_brand('HYN', 'HYUNDAI', 'Admin', null, null);


-- Create PRC soft delete
create or replace procedure delete_brand(
	in p_id integer,
	in p_deleted_by varchar,
	out out_stat boolean,
	out out_mess text
)
language plpgsql
as $$
begin
	if exists(
		select 1 from mst_brands
		where id = p_id and deleted_at is null
	) then
		update mst_brands mb 
			set deleted_at = now(),
			deleted_by = p_deleted_by,
			is_active = false
		where id = p_id;
		
		out_stat := true;
		out_mess := 'Data berhasil dihapus';
	else 
		out_stat := false;
		out_mess := 'Data tidak ditemukan.';
		return;
	
	end if;
end;
$$;


call delete_brand(6, 'Admin', null, null);

-- create view
create or replace view v_types_with_brand as 
select 
	t.id 	as type_id,
	t.name 	as type_name,
	t.code 	as type_code,
	b.id	as brand_id,
	b.name	as brand_name
from mst_types t
join mst_brands b on t.brand_id = b.id
where t.deleted_at is null 
and b.deleted_at is null;

select * from v_types_with_brand;


-- Create table mst_models
create table mst_models(
	id serial primary key,
	type_id integer not null references mst_types(id),
	code varchar(10) not null,
	name varchar(100) not null,
	year integer,	
	is_active boolean not null default true,
	created_at timestamp not null default now(),
	created_by varchar(100),
	updated_at timestamp,
	updated_by varchar(100),
	deleted_at timestamp,
	deleted_by varchar(100)
);


-- Insert data model
insert into mst_models(type_id, code, name, year, created_by)
values
	(8, 'JZ', 'Honda Jazz Juzz', 2025, 'Admin'),
	(9, 'CRV', 'Honda CRV Keluarga', 2024, 'Admin'),
	(10, 'MX', 'Toyota Mark XZY', 2026, 'Admin');

-- select join join
select 
	m.id	as model_id,
	m.name	as model_name,
	t.id	as type_id,
	t.name	as type_name,
	b.id	as brand_id,
	b.name	as brand_name
from mst_models m
join mst_types t on m.type_id = t.id
join mst_brands b on t.brand_id = b.id
where m.deleted_at is null
and t.deleted_at is null
and b.deleted_at is null;

-- procedure insert type
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
	if exists(
		select 1 from mst_brands
		where id = p_brand_id and deleted_at is null
	) then
		if exists(
			select 1 from mst_types
			where brand_id = p_brand_id 
				and code = p_code
				and deleted_at is null
		) then		
			out_stat := false;
			out_mess := 'Types code sudah ada: ' || p_code;
			return;
		else
			insert into mst_types(brand_id, code, name, created_by, created_at)
			values (p_brand_id, p_code, p_name, p_created_by, now());
			
			out_stat := true;
			out_mess := 'Data berhasil ditambahkan';
		end if;
	else 
		out_stat := false;
		out_mess := 'Brand id tidak ditemukan: ' || p_brand_id;
		return;
	end if;

end;
$$;


call insert_type(5, 'RX7', 'Mazda RX 7', 'Admin', null, null);

call insert_type(6, 'HY', 'Hyundai IONIC', 'Admin', null, null);

call insert_type(5, 'RX7', 'Mazda RX 7', 'Admin', null, null);


select * from mst_brands where deleted_at is null and id = 5;