import React from 'react';
import { Outlet } from 'react-router-dom';
import Navbar from './Navbar/Navbar';
import Footer from './footer/footer';
import Adminsidebar from './adminsidebar/adminsidebar';

const AdminLayout = () => {
  return (
    <div style={{ height: '100vh', display: 'flex', flexDirection: 'column' }}>
     
      <Navbar />

      <div style={{ display: 'flex', flex: 1 }}>
        
        <Adminsidebar />

        {/* Scrollable Main Content */}
        <main
          style={{
            flex: 1,
            padding: '20px',
            paddingTop: '100px', 
            background: '#e9e0e0',
            overflowY: 'auto', 
            height: 'calc(120vh - 140px)', 
          }}
        >
          <Outlet /> {/* Render child routes - Admindashboard, CategoryDetails... */}
        </main>
      </div>

      
      <Footer />
    </div>
  );
};

export default AdminLayout;
