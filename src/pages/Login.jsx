import { useRef, useState } from "react";
import styles from "./Loding.module.css";

const Login = () => {
    const emailRef = useRef(null);

    const handleSubmit = (event) =>{
        event.preventDefault();

        const emailValue = emailRef.current.value;
        if (!validation.isValidEmail(emailValue)) {
            setEmailError("Introduce un formato válido.");
            return;
        } else{
            setEmailError(null);
        }
    }
    return (
        <div className={styles.mainContainer}>
            <div>
                <h1>SOLESS</h1>
                <h2>Iniciar Sesión</h2>
                <form action="">
                    <div>
                        <input className={styles.input} type="email" placeholder="Email" />
                    </div>
                    <div>
                        <input type="password" placeholder="Contraseña" />
                    </div>
                    <div>
                        <div>
                            <input type="checkbox" name="" id="Recuerdame" />
                            <label htmlFor="Recuerdame">Recuerdame</label>
                        </div>
                        <span>Olvidaste tu contraseña?</span>
                    </div>
                    <button>Entrar</button>
                    <div>
                        ¿No tienes cuenta?<a>Regístrate!</a>
                    </div>
                </form>
            </div>
            <div className={styles.imagenLogin}>
                fjfjf
            </div>
        </div>
    );
};

export default Login;
