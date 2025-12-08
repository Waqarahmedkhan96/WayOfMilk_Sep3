-- ==========================================
-- CLEAN DEV DATA FOR WayOfMilk_T3
-- ==========================================

-- Tables are recreated every run (ddl-auto=create), so TRUNCATE is not strictly
-- needed, but it’s harmless if the tables already exist.

-- TRUNCATE TABLE transfer_record RESTART IDENTITY CASCADE;
-- TRUNCATE TABLE sale            RESTART IDENTITY CASCADE;
-- TRUNCATE TABLE milk            RESTART IDENTITY CASCADE;
-- TRUNCATE TABLE cow             RESTART IDENTITY CASCADE;
-- TRUNCATE TABLE customer        RESTART IDENTITY CASCADE;
-- TRUNCATE TABLE container       RESTART IDENTITY CASCADE;
-- TRUNCATE TABLE departments     RESTART IDENTITY CASCADE;
-- TRUNCATE TABLE users           RESTART IDENTITY CASCADE;

-- ===========================
-- 1) USERS
-- table users (
--   id, user_type, address, email, license_number, name, password, phone, role
-- )
-- ===========================

INSERT INTO users (name, email, phone, address, password, role, user_type, license_number) VALUES
                                                                                               ('Owner One',  'owner@farm.com',  '11111111', 'Farm Street 1',  'owner123',  'OWNER', 'OWNER',  NULL),
                                                                                               ('Worker One', 'worker@farm.com', '22222222', 'Farm Street 2',  'worker123', 'WORKER','WORKER', NULL),
                                                                                               ('Vet One',    'vet@farm.com',    '33333333', 'Vet Street 3',   'vet123',    'VET',   'VET',    'VET-12345');

-- IDs: 1 = Owner, 2 = Worker, 3 = Vet

-- ===========================
-- 2) DEPARTMENTS
-- table departments (id, type)
-- ===========================

INSERT INTO departments (type) VALUES
                                   ('RESTING'),     -- id = 1
                                   ('MILKING'),     -- id = 2
                                   ('QUARANTINE');  -- id = 3

-- ===========================
-- 3) CONTAINERS
-- table container (id, capacityl)
-- NOTE: column is "capacityl" (no underscore)
-- ===========================

INSERT INTO container (capacityl) VALUES
                                      (1000.0),  -- id = 1
                                      (2000.0);  -- id = 2

-- ===========================
-- 4) CUSTOMERS
-- table customer (
--   id, registered_by_id, company_name, companycvr, email, phone_no
-- )
-- ===========================

INSERT INTO customer (company_name, phone_no, email, companycvr, registered_by_id) VALUES
                                                                                       ('Supermarket A',    '44444444', 'contact@supera.com', '12345678', 1),
                                                                                       ('Cheese Factory B', '55555555', 'info@cheeseB.com',   '87654321', 1);

-- ===========================
-- 5) COWS
-- table cow (
--   id, birth_date, is_healthy, department_id, registered_by, reg_no
-- )
-- ===========================

INSERT INTO cow (reg_no, birth_date, is_healthy, department_id, registered_by) VALUES
                                                                                   ('DK-0001', '2021-03-10', TRUE,  2, 2), -- MILKING, Worker
                                                                                   ('DK-0002', '2020-11-05', TRUE,  2, 2),
                                                                                   ('DK-0003', '2019-07-21', FALSE, 3, 2); -- QUARANTINE

-- ===========================
-- 6) MILK
-- table milk (
--   approved_for_storage, date, volumel, container_id,
--   cow_id, id, registered_by_id, milk_test_result
-- )
-- NOTE: column is "volumel"
-- ===========================

INSERT INTO milk
(date,       volumel, milk_test_result, approved_for_storage, cow_id, container_id, registered_by_id)
VALUES
    ('2025-12-01', 500.0, 'PASS',    TRUE,  1, 1, 2),
    ('2025-12-01', 450.0, 'PASS',    TRUE,  2, 1, 2),
    ('2025-12-02', 300.0, 'FAIL',    FALSE, 3, 2, 2),
    ('2025-12-03', 700.0, 'UNKNOWN', TRUE,  1, 2, 2);

-- ===========================
-- 7) SALES
-- table sale (
--   price, quantityl, recall_case,
--   container_id, created_by_id, customer_id, date_time
-- )
-- NOTE: column is "quantityl"
-- ===========================

INSERT INTO sale
(created_by_id, date_time,            container_id, quantityl, price,   recall_case, customer_id)
VALUES
    (1,             '2025-12-02 10:00:00', 1,          400.0,     2500.0,  FALSE,       1),
    (1,             '2025-12-03 15:30:00', 2,          300.0,     1800.0,  FALSE,       2);

-- ===========================
-- 8) TRANSFER RECORDS
-- table transfer_record (
--   approved_by_vet_id, cow_id, department_id,
--   from_dept_id, id, moved_at, requested_by_id, to_dept_id
-- )
-- ===========================

INSERT INTO transfer_record
(moved_at,              from_dept_id, to_dept_id, department_id, requested_by_id, approved_by_vet_id, cow_id)
VALUES
    ('2025-12-01 08:00:00', 1,           2,          2,             2,               3,                 1),
    ('2025-12-02 09:30:00', 2,           3,          3,             2,               3,                 3);
