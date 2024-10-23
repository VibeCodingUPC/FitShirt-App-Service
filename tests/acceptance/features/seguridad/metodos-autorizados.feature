Feature: Métodos autorizados

  Como administrador,
  Quiero autorizar el acceso a ciertos métodos HTTP
  Para garantizar la seguridad de los datos y servicios de la aplicación.

  Scenario: Acceso autorizado a métodos HTTP protegidos
    Given que el usuario está autenticado correctamente
    When el usuario intenta acceder a un método HTTP protegido
    Then el acceso debería ser permitido.

  Scenario: Acceso no autorizado a métodos HTTP protegidos
    Given que el usuario no está autenticado
    When el usuario intenta acceder a un método HTTP protegido
    Then el acceso debería ser denegado con un mensaje "Acceso no autorizado".
