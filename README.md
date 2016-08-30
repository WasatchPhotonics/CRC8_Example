# CRC-8 Checksum Example

This repository provides a bare-bones working example of how to calculate the CRC8 checksum used in OEM WP Raman Spectrometers. 

![interface](images/interface.png)

The Cyclic Redunancy Check byte is used for error detection and is identical to that of Maxim/Dallas 1-wire devices, called *DOW CRC*. This calculation produces an 8-bit CRC value using the polynomial X^8 + X^5 + X^4 + X^0.

For more information, please refer to:
- [OEM API Specification for WP Raman Spectrometers](http://wasatchdevices.com/wp-content/uploads/2016/08/OEM-API-Specification.pdf)
- [Application Note 27: Understanding and Using Cyclic Redunancy Checks with Maxim iButton Production from Maxim Integrated](http://pdfserv.maximintegrated.com/en/an/AN27.pdf)