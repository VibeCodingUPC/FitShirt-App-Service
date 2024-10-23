Feature: Inicio de sesión de empresa

  Como representante de empresa registrado,
  Quiero poder iniciar sesión en mi cuenta de empresa
  Para administrar los servicios de mi empresa.

  Scenario: Inicio de sesión exitoso
    Given que el representante de empresa está en la página de inicio de sesión
    When el representante ingresa un correo válido y una contraseña válida
    And hace clic en el botón "Iniciar Sesión"
    Then debería ser redirigido al panel de usuario de empresa.

  Scenario: Inicio de sesión fallido por credenciales incorrectas
    Given que el representante de empresa está en la página de inicio de sesión
    When el representante ingresa un correo no registrado o una contraseña incorrecta
    And hace clic en el botón "Iniciar Sesión"
    Then debería ver un mensaje indicando "Credenciales incorrectas".
