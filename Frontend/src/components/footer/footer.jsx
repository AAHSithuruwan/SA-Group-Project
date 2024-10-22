import React from "react";
import { FaFacebook, FaInstagram, FaWhatsapp, FaEnvelope } from "react-icons/fa";
import { useNavigate } from 'react-router-dom'; // Import useNavigate for navigation
import './footer.css';

const Footer = () => {
  const navigate = useNavigate(); 

  return (
    <footer className="footer-container">
      <div className="footer-columns">

        {/* Contact Section */}
        <div className="footer-column">
          <h3>Contact</h3>
          <p>BidWave, NSBM, Pitipana, Homagama</p>
          <p>hi@bidwave.com</p>
          <div className="social-icons">
            <FaFacebook className="icon" />
            <FaInstagram className="icon" />
            <FaWhatsapp className="icon" />
            <FaEnvelope className="icon" />
          </div>
        </div>

        {/* Company Section */}
        <div className="footer-column">
          <h3>Company</h3>
          <ul>
            <li><a href="#">About Us</a></li>
            <li><a href="#">Tourz Reviews</a></li>
            <li><a href="#">Contact Us</a></li>
            <li><a href="#">Data Policy</a></li>
            <li><a href="#">Cookie Policy</a></li>
            <li><a href="#">Legal</a></li>
          </ul>
        </div>

        {/* Support Section */}
        <div className="footer-column">
          <h3>Support</h3>
          <ul>
            <li><a href="#">Get in Touch</a></li>
            <li><a href="#">Help Center</a></li>
            <li><a href="#">Live Chat</a></li>
            <li><a href="#">How it Works</a></li>
          </ul>
        </div>

        {/* Newsletter Section */}
        <div className="footer-column">
          <h3>Newsletter</h3>
          <p>Subscribe to the free newsletter and stay up to date</p>
          <div className="newsletter-form">
            <input type="email" placeholder="Your email address" className="email-input" />
            <button className="send-button">Send</button>
          </div>

          {/* Buttons below email input */}
          <div className="footer-buttons">
            <button className="footer-btn" onClick={() => navigate('/myaccount')}>Account</button>
            <button className="footer-btn" onClick={() => navigate('/sellerregistration')}>Register</button>
            <button className="footer-btn" onClick={() => navigate('/payment')}>Payment</button>
          </div>
        </div>

      </div>
      <div className="footer-bottom">
        <p>&copy; Copyright BidWave 2024</p>
      </div>
    </footer>
  );
};

export default Footer;
