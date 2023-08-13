use std::io::Write;

use base64::{engine::general_purpose, Engine};
use crypto_hash::{Algorithm, Hasher};
use libaes::Cipher;
use pbkdf2::pbkdf2_hmac_array;
use pqc_kyber::{encapsulate, KyberError};
use rand::{random, thread_rng};
use sha2::Sha512;

pub fn generate_salt() -> String {
    general_purpose::STANDARD.encode(random::<[u8; 16]>())
}

pub fn generate_basic_key(data: String, salt: String) -> String {
    let encryption_key = pbkdf2_hmac_array::<Sha512, 32>(data.as_bytes(), salt.as_bytes(), 1000);
    general_purpose::STANDARD.encode(encryption_key)
}

pub fn aes_encrypt(data: String, key: String) -> String {
    let iv = random::<[u8; 16]>();
    let cipher = Cipher::new_256(
        &general_purpose::STANDARD
            .decode(key)
            .unwrap()
            .try_into()
            .unwrap(),
    );
    let encrypted_data = cipher.cbc_encrypt(&iv, data.as_bytes());
    general_purpose::STANDARD.encode(iv) + ";" + &general_purpose::STANDARD.encode(&encrypted_data)
}

pub fn kyber_encrypt(data: String, public_key: String) -> Result<String, KyberError> {
    let public_key_bytes = general_purpose::STANDARD.decode(public_key).unwrap();

    let mut rng = thread_rng();

    let (encapsulation, secret) = encapsulate(&public_key_bytes, &mut rng)?;

    let salt = generate_salt();
    let encryption_key = generate_basic_key(general_purpose::STANDARD.encode(secret), salt.clone());

    let encrypted_data = aes_encrypt(data, encryption_key.clone());

    Ok(general_purpose::STANDARD.encode(encapsulation) + "|" + &salt + "|" + &encrypted_data)
}

pub fn hash(data: String) -> String {
    let mut hasher = Hasher::new(Algorithm::SHA256);
    let _ = hasher.write_all(data.as_bytes());
    let hash_result = hasher.finish();

    general_purpose::STANDARD.encode(hash_result)
}
