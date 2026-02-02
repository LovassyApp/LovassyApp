//! # Cryptography
//! contains code to (post-quantum) securely hash and encrypt sensitive user data

use base64::{Engine, engine::general_purpose};
use crypto_hash::{Algorithm, Hasher};
use libaes::Cipher;
use pbkdf2::pbkdf2_hmac_array;
use rand::random;
use safe_pqc_kyber::{KyberError, encapsulate};
use sha2::Sha512;
use std::io::Write;

pub fn generate_salt() -> String {
    general_purpose::STANDARD.encode(random::<[u8; 16]>())
}

pub fn generate_basic_key(data: &str, salt: &str) -> String {
    let encryption_key = pbkdf2_hmac_array::<Sha512, 32>(data.as_bytes(), salt.as_bytes(), 1000);
    general_purpose::STANDARD.encode(encryption_key)
}

pub fn aes_encrypt(data: &str, key: String) -> String {
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

pub fn kyber_encrypt(data: &str, public_key: String) -> Result<String, KyberError> {
    let public_key_bytes = general_purpose::STANDARD.decode(public_key).unwrap();

    let mut rng = rand::thread_rng();

    let (encapsulation, secret) = encapsulate(&public_key_bytes, &mut rng)?;

    let salt = generate_salt();
    let encryption_key = generate_basic_key(&general_purpose::STANDARD.encode(secret), &salt);

    let encrypted_data = aes_encrypt(data, encryption_key.clone());

    Ok(general_purpose::STANDARD.encode(encapsulation) + "|" + &salt + "|" + &encrypted_data)
}

/// `base64(sha256(data))`
pub fn hash(data: &str) -> String {
    let mut hasher = Hasher::new(Algorithm::SHA256);
    let _ = hasher.write_all(data.as_bytes());
    let hash_result = hasher.finish();

    general_purpose::STANDARD.encode(hash_result)
}
