export const API_BASE = 'https://localhost:7247/api';

export const API_ENDPOINTS = {
  AUTH: {
    REGISTER: `${API_BASE}/auth/register`,
    LOGIN: `${API_BASE}/auth/login`
  },
  RACES: {
    LIST: `${API_BASE}/races/list`,
    DETAIL: (id: number) => `${API_BASE}/races/${id}`,
    CREATE: `${API_BASE}/races`
  },
  REGISTRATION: {
    CREATE: `${API_BASE}/registrations`,
    USER_REGISTRATIONS: (userId: number) => `${API_BASE}/registrations/user/${userId}`
  },
  PAYMENTS: {
    CHECKOUT: `${API_BASE}/payments/checkout`,
    REFUND: `${API_BASE}/payments/refund`
  }
};
