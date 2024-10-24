import React from 'react';
import { Outlet } from 'react-router-dom';
import Navbar from './Navbar/Navbar';
import AdminSidebar from './adminsidebar/adminsidebar';

const AdminLayout = () => {
  return (
    <div style={{ height: '100vh', display: 'flex', background: '#e6f5ea' }}>
      {/* Navbar */}
      <Navbar />

      {/* Main Layout */}
      <div style={{ display: 'flex',flex: 1 }}>
        {/* Sidebar */}
        <AdminSidebar />

        {/* Scrollable Main Content */}
        <main
          style={{
            flex: 1,
            paddingTop: '80px',
            paddingLeft: '30px',
            paddingRight: '30px',
            backgroundColor: '#f0f9f3', 
            overflowY: 'auto',
            boxShadow: '0 4px 8px rgba(0, 0, 0, 0.1)', 
            borderRadius: '10px',
            marginLeft: '10px',
            marginRight: '10px', 
            minHeight: 'calc(100vh)', 
          }}
        >
          {/* Render child routes - Dashboard, etc */}
          <Outlet />
        </main>
      </div>

    </div>
  );
};

export default AdminLayout;
