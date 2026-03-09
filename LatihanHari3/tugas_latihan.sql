-- SOAL 1
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
)

insert into mst_models(type_id, code, name, year)
values
	(1, 'HBR', 'Hybrid', 2022),
	(1, 'ICE', 'Internal Combustion Engine', 2022),
	(1, 'EV', 'Electric Vehicle', 2022);

select b.name as brand_name, t.name as type_name, m.name as model_name, m.code, m.year
	from mst_models m join mst_types t on m.type_id = t.id
	join mst_brands b on t.brand_id = b.id
	where b.is_active = true
	and m.is_active = true
	and t.is_active = true
	order by b.name, t.name, m.name;


-- SOAL 2
create or replace procedure insert_type(
    in p_brand_id       int,
    in p_code           varchar(10),
    in p_name           varchar(100),
    in p_created_by     varchar(100),
    out out_stat        boolean,
    out out_message     text
)
language plpgsql
as $$
declare
    v_exists int;
begin
    select 1 into v_exists from mst_brands 
    where id = p_brand_id and is_active = true;

    if v_exists is null then
        out_stat := false;
        out_message := 'Brand tidak ditemukan atau tidak aktif.';
        return;
    end if;


    v_exists := null;
    select 1 into v_exists from mst_types 
    where code = p_code and brand_id = p_brand_id;

    if v_exists is not null then
        out_stat := false;
        out_message := 'Kombinasi brand dan type yang diinputkan sudah tersedia.';
        return;
    end if;

    insert into mst_types (brand_id, code, "name", created_by, created_at)
    values (p_brand_id, p_code, p_name, p_created_by, now());

    out_stat := true;
    out_message := 'Berhasil menambahkan type.';
end;
$$;


call insert_type(10, 'JAZZ', 'Honda Jazz', 'Admin', null, null);
