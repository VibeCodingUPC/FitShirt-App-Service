Feature: Inicio de sesión de cliente

  Como cliente registrado,
  Quiero poder iniciar sesión en mi cuenta de cliente
  Para acceder a mi panel de usuario.

  Scenario: Inicio de sesión exitoso
    Given que el cliente está en la página de inicio de sesión
    When el cliente ingresa un correo válido y una contraseña válida
    And hace clic en el botón "Iniciar Sesión"
    Then debería ser redirigido al panel de usuario de cliente.

  Scenario: Inicio de sesión fallido por credenciales incorrectas
    Given que el cliente está en la página de inicio de sesión
    When el cliente ingresa un correo no registrado o una contraseña incorrecta
    And hace clic en el botón "Iniciar Sesión"
    Then debería ver un mensaje indicando "Credenciales incorrectas".
