create table mst_brands( 
id serial primary key, 
code varchar(10) not null, 
name varchar(100) not null,
is_active boolean not null default true, 
created_at timestamp not null default now(),
created_by varchar (100), 
updated_at timestamp,
updated_by varchar(100), 
deleted_at timestamp, 
deleted_by varchar(100)
)

-- insert brand
insert into mst_brands (code, name, created_by)
values ('HON', 'Honda', 'Admin')

--insert bulk 
insert into mst_brands (code, name, created_by)
values 
	('TOY', 'Toyota', 'Admin'),
	('SUZ', 'Suzuki', 'Admin'),
	('DAI', 'Daihatsu', 'Admin');
	
-- select teble 
select * from mst_brands 


-- yang beum di delet
select * from mst_brands where deleted_by is null

--seoectm + filter + sort
select id, name, is_active
from mst_brands mb where deleted_at is null
order by name asc

--pencaraian case insensitive
select * from mst_brands 
where deleted_at is null
and name ilike '%toy%';

-- count
select count (*) from mst_brands mb where mb.deleted_at is null;

--update 
update mst_brands mb 
set name = 'Honda Motor Indonesia',
	updated_at= now(),
	updated_by = 'Admin'
where id = 1
and deleted_at is null

--soft delete

update mst_brands mb 
set deleted_at = now(),
	deleted_by = 'Admin'
where id = 1

--create table mst_type 
create table mst_types(
	id serial primary key,
	brand_id integer not null references mst_brands (id),
	code varchar(10) not null, 
	name varchar(100) not null,
	is_active boolean not null default true, 
	created_at timestamp not null default now(),
	created_by varchar (100), 
	updated_at timestamp,
	updated_by varchar(100), 
	deleted_at timestamp, 
	deleted_by varchar(100)
)

insert into mst_types(brand_id, code, name, created_by)
values
(1,'JAZZ', 'Honda Jazz','Admin'),
(1,'CRV','Honda Crv','Admin'),
(2,'MX', 'Toyota Mark X', 'Admin')

•--select + join
select
t.id as type_id,
t.code as type_code,
t.name as type_name, 
b.name as brand_name
from mst_types t
join mst_brands b on t.brand_id = b.id
where t.deleted_at is null
and b.deleted_at is null order by b.name, t.name;


CREATE OR REPLACE PROCEDURE insert_brand(
    IN p_code VARCHAR, 
    IN p_name VARCHAR, 
    IN p_created_by VARCHAR, 
    OUT out_stat BOOLEAN, 
    OUT out_mess TEXT
)
LANGUAGE plpgsql 
AS $$
BEGIN
    -- 1. Cek duplikat
    IF EXISTS (
        SELECT 1 FROM mst_brands 
        WHERE code = p_code AND deleted_at IS NULL
    ) THEN
        out_stat := FALSE;
        out_mess := 'Kode sudah dipakai: ' || p_code;
        RETURN;
    END IF;

    -- 2. Proses Insert
    INSERT INTO mst_brands (code, name, created_by, created_at) 
    VALUES (p_code, p_name, p_created_by, NOW());

    -- 3. Set status sukses
    out_stat := TRUE;
    out_mess := 'Data berhasil disimpan';
END;
$$;

call insert_brand ('MZD', 'Mazda', 'Admin',null,null)

create or replace procedure prc_soft_delete(
	in p_id int,
	in p_deleted_at varchar,
	out out_mess,
	out out_state
)

language plglsql
as $$
begin
	if exist (
	select 1 from mst_brand
	where p_deleted_at n not null
)
end
$$
end

--view dengan join 
create or replace view v_types_with_brand as 
select 
	t.id	as type_id,
	t.name 	as type_name,
	t.code	as typ_code,
	b.id 	as brand_id,
	b.name 	as brand_name
from mst_types t
join mst_brands b on t.brand_id = b.id
where t.deleted_at is null
and b.deleted_at is null;

select * from v_types_with_brand


--latihan 1
create table mst_models(
id serial primary key, 
type_id integer not null references mst_types (id),
code varchar(10) not null, 
name varchar(100) not null,
year integer not null,
is_active boolean not null default true, 
created_at timestamp not null default now(),
created_by varchar (100), 
updated_at timestamp,
updated_by varchar(100), 
deleted_at timestamp, 
deleted_by varchar(100)
)

insert into mst_models  (type_id , code, name,year, created_by)
values  
	(2,'TOY', 'Toyota',2020, 'Admin'),
	(2,'SUZ', 'Suzuki', 2024,'Admin'),
	(1,'DAI', 'Daihatsu',2034, 'Admin');


select 
m.id as model_id,
m.name as model_name,
m.code as model_code,
m.year as model_year,
t.id as type_id,
t.name as type_name,
t.code as type_code,
b.id as brand_id,
b.name as brand_name
from mst_models m 
join mst_brands b 
on m.type_id  = b.id
join mst_types t
on t.brand_id = b.id
where t.deleted_at is null and b.deleted_at is null and m.deleted_at is null

--latihan 2 buat prosedure prc_insert_type(pbrand_id, p_code, p_name,p_created_by, out stat, out mess) validasi : harus ada brand_id di mst_type, code+brand_id tidak boleh duplikat 
CREATE OR REPLACE PROCEDURE prc_insert_type(
    IN p_brand_id INT, 
    IN p_code VARCHAR, 
    IN p_name VARCHAR, 
    IN p_created_by VARCHAR, 
    OUT out_stat BOOLEAN, 
    OUT out_mess TEXT
)
LANGUAGE plpgsql 
AS $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM mst_brands 
        WHERE id = p_brand_id AND deleted_at IS NULL
    ) THEN
        out_stat := FALSE;
        out_mess := 'Gagal: Brand ID tidak ditemukan.';
        RETURN;
    END IF;
    IF EXISTS (
        SELECT 1 FROM mst_types 
        WHERE code = p_code 
          AND brand_id = p_brand_id 
          AND deleted_at IS NULL
    ) THEN
        out_stat := FALSE;
        out_mess := 'Gagal: Kode ' || p_code || ' sudah terdaftar untuk Brand ini.';
        RETURN;
    END IF;
    INSERT INTO mst_types (brand_id, code, name, created_by, created_at)
    VALUES (p_brand_id, p_code, p_name, p_created_by, NOW());
    out_stat := TRUE;
    out_mess := 'Berhasil: Data Type telah ditambahkan.';

EXCEPTION WHEN OTHERS THEN
    out_stat := FALSE;
    out_mess := 'Error: ' || SQLERRM;
END;
$$;

-- call berhasil 
call prc_insert_type (2,'HON','motor bebek','Admin',null,null)
--call duplikat 
CALL prc_insert_type( 3,'TOY', 'Avanza Veloz',  'Admin', NULL, NULL);
CALL prc_insert_type( 3,'TOY', 'Avanza Veloz',  'Admin', NULL, NULL);

--call id tidak ditemukan jika brand id is deleted
CALL prc_insert_type( 1,'SUZ', 'phanter',  'Admin', NULL, NULL);










